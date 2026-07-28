using Core.Article.Entity;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ArticleCommentRepository(AppDbContext context)
        : GenericRepository<ArticleCommentEntity>(context), IArticleCommentRepository
    {
        public async Task<List<ArticleCommentEntity>> GetByArticleId(Guid articleId)
        {
            return await context.Set<ArticleCommentEntity>()
                .Where(c => c.ArticleId == articleId)
                .OrderBy(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<ArticleCommentEntity?> GetByArticleIdAndCommentId(Guid articleId, Guid commentId)
        {
            return await context.Set<ArticleCommentEntity>()
                .FirstOrDefaultAsync(c => c.ArticleId == articleId && c.Id == commentId);
        }

        public async Task<List<ArticleCommentEntity>> GetReplies(Guid parentCommentId)
        {
            return await context.Set<ArticleCommentEntity>()
                .Where(c => c.ParentCommentId == parentCommentId)
                .ToListAsync();
        }
    }
}
