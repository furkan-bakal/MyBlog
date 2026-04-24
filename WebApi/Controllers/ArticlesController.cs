using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticleService _articlesService;
        public ArticlesController()
        {
            _articlesService = new ArticleService();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var articles = _articlesService.GetAll();
            return Ok(articles);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ArticleEntity article)
        {
            _articlesService.Add(article);
            return CreatedAtAction(nameof(Get), new { id = _articlesService.GetAll().Count - 1 }, article);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var article = _articlesService.GetById(id);
            return Ok(article);
        }
    }
}
