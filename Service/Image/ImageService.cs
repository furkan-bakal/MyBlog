using Core;
using Core.Image.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Service
{
    /// <summary>
    /// Yazı içeriğine gömülen görselleri yönetir. Makale görsellerinden farklı olarak bir makaleye
    /// bağlı değildir (editörde makale kaydedilmeden de yüklenebilir), bu yüzden veritabanında
    /// kaydı tutulmaz; sadece diske yazılıp servis edilebilir yolu döndürülür.
    /// </summary>
    public class ImageService : IImageService
    {
        private const string UploadFolder = "uploads/content";

        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<ResponseModelDto<ContentImageDto>> Upload(IFormFile file)
        {
            if (file is null)
            {
                return ResponseModelDto<ContentImageDto>.Failure("Bir görsel yüklenmelidir.");
            }

            var failMessages = ImageFileRules.Validate(file).ToList();
            if (failMessages.Count > 0)
            {
                return ResponseModelDto<ContentImageDto>.Failure(failMessages);
            }

            var targetDirectory = Path.Combine(
                ImageFileRules.GetWebRootPath(_environment),
                UploadFolder.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(targetDirectory);

            var fileName = ImageFileRules.BuildStoredFileName(file);
            var physicalPath = Path.Combine(targetDirectory, fileName);

            await using (var stream = new FileStream(physicalPath, FileMode.CreateNew))
            {
                await file.CopyToAsync(stream);
            }

            var dto = new ContentImageDto(
                FileName: fileName,
                OriginalFileName: Path.GetFileName(file.FileName),
                Path: $"/{UploadFolder}/{fileName}",
                ContentType: file.ContentType,
                FileSize: file.Length);

            return ResponseModelDto<ContentImageDto>.Success(dto, HttpStatusCode.Created);
        }
    }
}
