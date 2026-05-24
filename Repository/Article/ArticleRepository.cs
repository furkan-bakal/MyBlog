using Core;
using Core.Article.Entity;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ArticleRepository(AppDbContext context) : GenericRepository<ArticleEntity>(context), IArticleRepository
    {
        public async Task<List<ArticleEntity>> GetAllByPaginate(int take, int skip)
        {
            return await context.Set<ArticleEntity>()
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<ArticleEntity?> GetByIdWithCategory(Guid id)
        {
            return await context.Set<ArticleEntity>()
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
