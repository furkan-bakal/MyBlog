
using Core;
using Core.Article.Dto;
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
            return CreateActionResult(await _articleService.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateArticleDto createArticleDto)
        {
            var result = await _articleService.Add(createArticleDto);
            return CreateActionResult(result, nameof(GetById), new { id = result.Data });
        }

        [HttpGet("{id:guid}")]
        [ServiceFilter(typeof(NotFoundFilter))]
        public async Task<IActionResult> GetById(Guid id)
        {
            return CreateActionResult(await _articleService.GetById(id));
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateArticleDto updateArticleDto)
        {
            return CreateActionResult(await _articleService.Update(id, updateArticleDto));
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return CreateActionResult(await _articleService.Remove(id));
        }

        [HttpGet]
        [Route("getpaginate")]
        public async Task<IActionResult> GetAllByPaginate([FromQuery] int take, [FromQuery] int skip)
        {
            return CreateActionResult(await _articleService.GetAllByPaginate(take, skip));
        }
    }
}