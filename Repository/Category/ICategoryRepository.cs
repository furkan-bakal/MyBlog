using Core.Category.Entity;

namespace Repository.Category
{
    public interface ICategoryRepository: IGenericRepository<CategoryEntity>
    {
        Task<CategoryEntity?> GetByIdWithArticles(Guid id);
    }
}
