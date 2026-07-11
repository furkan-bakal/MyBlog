using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Service
{
    /// <summary>
    /// Görsel yüklemelerinin ortak kuralları ve disk yardımcıları. Hem makale görselleri
    /// (<see cref="ArticleImageService"/>) hem de içerik görselleri (<see cref="ImageService"/>)
    /// aynı doğrulamayı kullanır.
    /// </summary>
    public static class ImageFileRules
    {
        public const long MaxFileSizeInBytes = 5 * 1024 * 1024;

        public static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp", ".gif"];
        public static readonly string[] AllowedContentTypes = ["image/jpeg", "image/png", "image/webp", "image/gif"];

        public static IEnumerable<string> Validate(IFormFile file)
        {
            if (file.Length == 0)
            {
                yield return $"{file.FileName}: Dosya boş.";
                yield break;
            }

            if (file.Length > MaxFileSizeInBytes)
            {
                yield return $"{file.FileName}: Dosya boyutu en fazla {MaxFileSizeInBytes / (1024 * 1024)} MB olabilir.";
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                yield return $"{file.FileName}: Sadece {string.Join(", ", AllowedExtensions)} uzantılı dosyalar yüklenebilir.";
            }

            if (!AllowedContentTypes.Contains(file.ContentType.ToLowerInvariant()))
            {
                yield return $"{file.FileName}: Geçersiz içerik tipi ({file.ContentType}).";
            }
        }

        /// <summary>Diskte saklanacak çakışmasız dosya adı (orijinal uzantı korunur).</summary>
        public static string BuildStoredFileName(IFormFile file)
        {
            return $"{Guid.NewGuid()}{Path.GetExtension(file.FileName).ToLowerInvariant()}";
        }

        public static string GetWebRootPath(IWebHostEnvironment environment)
        {
            var webRootPath = environment.WebRootPath ?? Path.Combine(environment.ContentRootPath, "wwwroot");
            Directory.CreateDirectory(webRootPath);
            return webRootPath;
        }

        public static void TryDeleteFile(string physicalPath)
        {
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }
        }
    }
}
