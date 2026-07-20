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

        public async Task<int> IncrementViewCount(Guid id)
        {
            // ExecuteUpdateAsync change tracker'ı atlar ve tek UPDATE gönderir;
            // eşzamanlı isteklerde sayaç kaybı olmaz. Global query filter sayesinde
            // silinmiş makaleler zaten güncellenmez.
            return await context.Set<ArticleEntity>()
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(setters => setters.SetProperty(a => a.ViewCount, a => a.ViewCount + 1));
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
