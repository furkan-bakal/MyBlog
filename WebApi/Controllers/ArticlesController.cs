using Core;
using Core.ArticleCreateUseCase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using WebApi.Filters;

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
        public async Task<IActionResult> Get()
        {
            return CreateActionResult(await _articleService.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateArticleDto createArticleDto)
        {
            var result = await _articleService.Add(createArticleDto);
            return CreateActionResult(result, nameof(GetById), new { id = result.Data });
        }

        [HttpGet("{id:int}")]
        [ServiceFilter(typeof(NotFoundFilter))]
        public async Task<IActionResult> GetById(int id)
        {
            return CreateActionResult(await _articleService.GetById(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateArticleDto updateArticleDto)
        {
            return CreateActionResult(await _articleService.Update(id, updateArticleDto));
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResult(await _articleService.Remove(id));
        }
    }
}