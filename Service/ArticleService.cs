using AutoMapper;
using Core;
using Core.ArticleCreateUseCase;
using Repository;
using System.Collections.Immutable;
using System.Net;

namespace Service
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public ArticleService(IArticleRepository articleRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _articleRepository = articleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModelDto<IImmutableList<ArticleDto>>> GetAll()
        {
            var articles = (await _articleRepository.GetAll()).ToList();
            
            var tranformedArticles = _mapper.Map<List<ArticleEntity>, List<ArticleDto>>(articles).ToImmutableList();
            return ResponseModelDto<IImmutableList<ArticleDto>>.Success(tranformedArticles);
        }

        public async Task<ResponseModelDto<int>> Add(CreateArticleDto createArticleDto)
        {
            var entity = new ArticleEntity
            {
                Content = createArticleDto.Content,
                Title = createArticleDto.Title,
                Author = createArticleDto.Author,
                CreatedDate = DateTime.UtcNow
            };
            await _articleRepository.Add(entity);
            await _unitOfWork.CommitAsync();
            return ResponseModelDto<int>.Success(entity.Id);
        }

        public async Task<ResponseModelDto<ArticleDto?>> GetById(int id)
        {
            var article = await _articleRepository.GetById(id);

            var articleDto = _mapper.Map<ArticleEntity, ArticleDto>(article!);
            return ResponseModelDto<ArticleDto?>.Success(articleDto);
        }

        public async Task<ResponseModelDto<NoContent>> Remove(int id)
        {
            var article = await _articleRepository.GetById(id);
            if (article is null)
            {
                return ResponseModelDto<NoContent>.Failure("Article not found", HttpStatusCode.NotFound);
            }
            await _articleRepository.Remove(article);
            await _unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        public async Task<ResponseModelDto<NoContent>> Update(int id, UpdateArticleDto updateArticleDto)
        {
            var article = await _articleRepository.GetById(id);
            if (article is null)
            {
                return ResponseModelDto<NoContent>.Failure("Article not found", HttpStatusCode.NotFound);
            }

            article.Content = updateArticleDto.Content;
            article.Title = updateArticleDto.Title;
            article.Author = updateArticleDto.Author;
            article.UpdatedDate = DateTime.UtcNow;

            await _articleRepository.Update(article);
            await _unitOfWork.CommitAsync();
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }
    }
}