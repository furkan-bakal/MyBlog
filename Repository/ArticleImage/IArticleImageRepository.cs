using Core.Article.Entity;

namespace Repository
{
    public interface IArticleImageRepository : IGenericRepository<ArticleImageEntity>
    {
        Task<List<ArticleImageEntity>> GetByArticleId(Guid articleId);

        Task<ArticleImageEntity?> GetByArticleIdAndImageId(Guid articleId, Guid imageId);
    }
}
