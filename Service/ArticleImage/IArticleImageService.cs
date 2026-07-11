using Core;
using Core.Article.Dto;
using Microsoft.AspNetCore.Http;
using System.Collections.Immutable;

namespace Service
{
    public interface IArticleImageService
    {
        Task<ResponseModelDto<IImmutableList<ArticleImageDto>>> Upload(Guid articleId, IReadOnlyList<IFormFile> files);
        Task<ResponseModelDto<IImmutableList<ArticleImageDto>>> GetByArticleId(Guid articleId);
        Task<ResponseModelDto<NoContent>> Remove(Guid articleId, Guid imageId);
    }
}
