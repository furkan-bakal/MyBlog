using Core;
using Core.Article.Dto;

namespace Service
{
    public interface IArticleLikeService
    {
        Task<ResponseModelDto<ArticleLikeStatusDto>> GetStatus(Guid articleId);
        Task<ResponseModelDto<ArticleLikeStatusDto>> Like(Guid articleId);
        Task<ResponseModelDto<ArticleLikeStatusDto>> Unlike(Guid articleId);
    }
}
