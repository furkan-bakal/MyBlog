using AutoMapper;
using Core;
using Core.Article.Dto;
using Core.Article.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Repository;
using System.Collections.Immutable;
using System.Net;

namespace Service
{
    public class ArticleImageService : IArticleImageService
    {
        private const string UploadFolder = "uploads/articles";

        private readonly IArticleImageRepository _articleImageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public ArticleImageService(
            IArticleImageRepository articleImageRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IWebHostEnvironment environment)
        {
            _articleImageRepository = articleImageRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<ResponseModelDto<IImmutableList<ArticleImageDto>>> Upload(Guid articleId, IReadOnlyList<IFormFile> files)
        {
            if (files is null || files.Count == 0)
            {
                return ResponseModelDto<IImmutableList<ArticleImageDto>>.Failure("En az bir görsel yüklenmelidir.");
            }

            var failMessages = files.SelectMany(ImageFileRules.Validate).ToList();
            if (failMessages.Count > 0)
            {
                return ResponseModelDto<IImmutableList<ArticleImageDto>>.Failure(failMessages);
            }

            var targetDirectory = Path.Combine(
                ImageFileRules.GetWebRootPath(_environment),
                UploadFolder.Replace('/', Path.DirectorySeparatorChar),
                articleId.ToString());
            Directory.CreateDirectory(targetDirectory);

            var entities = new List<ArticleImageEntity>();
            var writtenFiles = new List<string>();

            try
            {
                foreach (var file in files)
                {
                    var fileName = ImageFileRules.BuildStoredFileName(file);
                    var physicalPath = Path.Combine(targetDirectory, fileName);

                    await using (var stream = new FileStream(physicalPath, FileMode.CreateNew))
                    {
                        await file.CopyToAsync(stream);
                    }
                    writtenFiles.Add(physicalPath);

                    entities.Add(new ArticleImageEntity
                    {
                        ArticleId = articleId,
                        FileName = fileName,
                        OriginalFileName = Path.GetFileName(file.FileName),
                        Path = $"/{UploadFolder}/{articleId}/{fileName}",
                        ContentType = file.ContentType,
                        FileSize = file.Length
                    });
                }

                foreach (var entity in entities)
                {
                    await _articleImageRepository.Add(entity);
                }

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                // DB kaydı başarısız olursa diskte yetim dosya bırakmayalım
                foreach (var path in writtenFiles)
                {
                    ImageFileRules.TryDeleteFile(path);
                }
                throw;
            }

            var dtos = _mapper.Map<List<ArticleImageEntity>, List<ArticleImageDto>>(entities).ToImmutableList();
            return ResponseModelDto<IImmutableList<ArticleImageDto>>.Success(dtos, HttpStatusCode.Created);
        }

        public async Task<ResponseModelDto<IImmutableList<ArticleImageDto>>> GetByArticleId(Guid articleId)
        {
            var images = await _articleImageRepository.GetByArticleId(articleId);

            var dtos = _mapper.Map<List<ArticleImageEntity>, List<ArticleImageDto>>(images).ToImmutableList();
            return ResponseModelDto<IImmutableList<ArticleImageDto>>.Success(dtos);
        }

        public async Task<ResponseModelDto<NoContent>> Remove(Guid articleId, Guid imageId)
        {
            var image = await _articleImageRepository.GetByArticleIdAndImageId(articleId, imageId);
            if (image is null)
            {
                return ResponseModelDto<NoContent>.Failure($"Image with id {imageId} not found for article {articleId}.", HttpStatusCode.NotFound);
            }

            await _articleImageRepository.Remove(image);
            await _unitOfWork.CommitAsync();

            // Kayıt soft-delete edildi; dosya diskte kalırsa URL'i hâlâ servis edileceği için fiziksel dosyayı da siliyoruz.
            ImageFileRules.TryDeleteFile(Path.Combine(
                ImageFileRules.GetWebRootPath(_environment),
                image.Path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar)));

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }
    }
}
