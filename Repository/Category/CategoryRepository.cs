using Core.Category.Entity;

namespace Repository.Category
{
    public class CategoryRepository(AppDbContext dbContext): GenericRepository<CategoryEntity>(dbContext), ICategoryRepository
    {
    }
}
