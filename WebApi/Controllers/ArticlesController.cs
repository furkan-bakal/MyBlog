
using Core;
using Microsoft.AspNetCore.Authorization;
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
            throw new Exception("Test exception for global handler");
            return CreateActionResult(await _articleService.GetAll());
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateArticleDto createArticleDto)
        {
            var result = await _articleService.Add(createArticleDto);
            return CreateActionResult(result, nameof(GetById), new { id = result.Data });
        }

        [Authorize()]
        [HttpGet("{id:int}")]
        [ServiceFilter(typeof(NotFoundFilter))]
        public async Task<IActionResult> GetById(int id)
        {
            return CreateActionResult(await _articleService.GetById(id));
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateArticleDto updateArticleDto)
        {
            return CreateActionResult(await _articleService.Update(id, updateArticleDto));
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResult(await _articleService.Remove(id));
        }
    }
}