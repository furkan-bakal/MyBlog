using Core;
using Core.Article.Entity;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ArticleRepository(AppDbContext context) : GenericRepository<ArticleEntity>(context), IArticleRepository
    {
        public override async Task<IReadOnlyList<ArticleEntity>> GetAll()
        {
            var list = await context.Set<ArticleEntity>()
                .Include(a => a.Images)
                .ToListAsync();

            return list.AsReadOnly();
        }

        public async Task<List<ArticleEntity>> GetAllByPaginate(int take, int skip)
        {
            return await context.Set<ArticleEntity>()
                .Include(a => a.Images)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<ArticleEntity?> GetByIdWithCategory(Guid id)
        {
            return await context.Set<ArticleEntity>()
                .Include(a => a.Category)
                .Include(a => a.Images)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
