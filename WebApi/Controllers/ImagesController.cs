using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : CustomBaseController
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        /// <summary>Yazı içeriğine gömmek üzere tek bir görsel yükler; servis edilebilir yolu döner.</summary>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            return CreateActionResult(await _imageService.Upload(file));
        }
    }
}
