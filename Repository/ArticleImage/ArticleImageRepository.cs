using Core.Article.Entity;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ArticleImageRepository(AppDbContext context) : GenericRepository<ArticleImageEntity>(context), IArticleImageRepository
    {
        public async Task<List<ArticleImageEntity>> GetByArticleId(Guid articleId)
        {
            return await context.Set<ArticleImageEntity>()
                .Where(i => i.ArticleId == articleId)
                .OrderBy(i => i.CreatedDate)
                .ToListAsync();
        }

        public async Task<ArticleImageEntity?> GetByArticleIdAndImageId(Guid articleId, Guid imageId)
        {
            return await context.Set<ArticleImageEntity>()
                .FirstOrDefaultAsync(i => i.ArticleId == articleId && i.Id == imageId);
        }
    }
}
