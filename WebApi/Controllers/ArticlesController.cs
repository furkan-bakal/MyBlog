using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : CustomBaseController
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return CreateActionResult(_articleService.GetAll());
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateArticleDto createArticleDto)
        {
            var result = _articleService.Add(createArticleDto);
            return CreateActionResult(result, nameof(GetById), new { id = result.Data });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return CreateActionResult(_articleService.GetById(id));
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateArticleDto updateArticleDto)
        {
            return CreateActionResult(_articleService.Update(id, updateArticleDto));
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            return CreateActionResult(_articleService.Remove(id));
        }
    }
}