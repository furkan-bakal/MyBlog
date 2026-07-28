using Core;
using Core.Article.Dto;
using System.Collections.Immutable;

namespace Service
{
    public interface IArticleCommentService
    {
        Task<ResponseModelDto<IImmutableList<ArticleCommentDto>>> GetByArticleId(Guid articleId);
        Task<ResponseModelDto<Guid>> Add(Guid articleId, CreateArticleCommentDto createArticleCommentDto);
        Task<ResponseModelDto<NoContent>> Remove(Guid articleId, Guid commentId);
    }
}
