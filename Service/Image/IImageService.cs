using Core;
using Core.Image.Dto;
using Microsoft.AspNetCore.Http;

namespace Service
{
    public interface IImageService
    {
        /// <summary>Makaleye bağlı olmayan tek bir içerik görselini yükler.</summary>
        Task<ResponseModelDto<ContentImageDto>> Upload(IFormFile file);
    }
}
