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
        private readonly ArticleService _articlesService = new();

        [HttpGet]
        public IActionResult Get()
        {
            var articles = _articlesService.GetAll();
            return Ok(articles);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ArticleDto article)
        {
            var result = _articlesService.Add(article);
            return CreatedAtAction(nameof(GetById), new { id = _articlesService.GetAll().Data.Count - 1 }, article);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var article = _articlesService.GetById(id);

            if (!article.IsSuccess)
            {
                return NotFound(article.FailMessages);
            }

            return Ok(article);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            _articlesService.Remove(id);
            return NoContent();
        }
    }
}