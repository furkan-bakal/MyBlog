using Core;

namespace Repository
{
    //Servisler genericleştirilmez.
    public interface IGenericRepository<T> where T : BaseEntity<Guid>
    {
        Task<T> Add(T entity);
        Task<IReadOnlyList<T>> GetAll();
        Task<T?> GetById(Guid Id);
        Task Remove(T entity);
        Task Update(T entity);
        Task<bool> HasExist(Guid id);
    }
}
