using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new[] { "Article 1", "Article 2", "Article 3" });
        }
    }
}
