
using Core;
using Core.Article.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Service;
using WebApi.Extensions;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : CustomBaseController
    {
        private readonly IArticleService _articleService;
        private readonly IArticleImageService _articleImageService;
        private readonly IArticleLikeService _articleLikeService;
        private readonly IArticleCommentService _articleCommentService;

        public ArticlesController(
            IArticleService articleService,
            IArticleImageService articleImageService,
            IArticleLikeService articleLikeService,
            IArticleCommentService articleCommentService)
        {
            _articleService = articleService;
            _articleImageService = articleImageService;
            _articleLikeService = articleLikeService;
            _articleCommentService = articleCommentService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return CreateActionResult(await _articleService.GetAll());
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
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

        [HttpGet("{id:guid}/images")]
        [ServiceFilter(typeof(NotFoundFilter))]
        public async Task<IActionResult> GetImages(Guid id)
        {
            return CreateActionResult(await _articleImageService.GetByArticleId(id));
        }

        [HttpPost("{id:guid}/images")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(NotFoundFilter))]
        public async Task<IActionResult> UploadImages(Guid id, [FromForm] List<IFormFile> files)
        {
            return CreateActionResult(await _articleImageService.Upload(id, files));
        }

        [HttpDelete("{id:guid}/images/{imageId:guid}")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(NotFoundFilter))]
        public async Task<IActionResult> DeleteImage(Guid id, Guid imageId)
        {
            return CreateActionResult(await _articleImageService.Remove(id, imageId));
        }

        [HttpGet("{id:guid}/like")]
        [ServiceFilter(typeof(NotFoundFilter))]
        public async Task<IActionResult> GetLikeStatus(Guid id)
        {
            return CreateActionResult(await _articleLikeService.GetStatus(id));
        }

        [HttpPost("{id:guid}/like")]
        [EnableRateLimiting(RateLimitPolicies.ArticleLike)]
        [ServiceFilter(typeof(NotFoundFilter))]
        public async Task<IActionResult> Like(Guid id)
        {
            return CreateActionResult(await _articleLikeService.Like(id));
        }

        [HttpDelete("{id:guid}/like")]
        [EnableRateLimiting(RateLimitPolicies.ArticleLike)]
        [ServiceFilter(typeof(NotFoundFilter))]
        public async Task<IActionResult> Unlike(Guid id)
        {
            return CreateActionResult(await _articleLikeService.Unlike(id));
        }

        [HttpGet("{id:guid}/comments")]
        [ServiceFilter(typeof(NotFoundFilter))]
        public async Task<IActionResult> GetComments(Guid id)
        {
            return CreateActionResult(await _articleCommentService.GetByArticleId(id));
        }

        [HttpPost("{id:guid}/comments")]
        [EnableRateLimiting(RateLimitPolicies.ArticleComment)]
        [ServiceFilter(typeof(NotFoundFilter))]
        public async Task<IActionResult> PostComment(Guid id, [FromBody] CreateArticleCommentDto createArticleCommentDto)
        {
            return CreateActionResult(await _articleCommentService.Add(id, createArticleCommentDto));
        }

        [HttpDelete("{id:guid}/comments/{commentId:guid}")]
        [Authorize(Roles = "admin")]
        [ServiceFilter(typeof(NotFoundFilter))]
        public async Task<IActionResult> DeleteComment(Guid id, Guid commentId)
        {
            return CreateActionResult(await _articleCommentService.Remove(id, commentId));
        }
    }
}