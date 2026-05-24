using Core.Category.Entity;
using Microsoft.EntityFrameworkCore;

namespace Repository.Category
{
    public class CategoryRepository(AppDbContext dbContext) : GenericRepository<CategoryEntity>(dbContext), ICategoryRepository
    {
        public async Task<CategoryEntity?> GetByIdWithArticles(Guid id)
        {
            return await dbContext.Categories
                .Include(c => c.Articles)
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();
        }
    }
}
