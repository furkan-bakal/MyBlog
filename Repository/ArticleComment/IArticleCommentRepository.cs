using Core.Article.Entity;

namespace Repository
{
    public interface IArticleCommentRepository : IGenericRepository<ArticleCommentEntity>
    {
        public Task<List<ArticleCommentEntity>> GetByArticleId(Guid articleId);

        public Task<ArticleCommentEntity?> GetByArticleIdAndCommentId(Guid articleId, Guid commentId);

        public Task<List<ArticleCommentEntity>> GetReplies(Guid parentCommentId);
    }
}
