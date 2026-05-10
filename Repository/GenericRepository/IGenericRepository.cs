namespace Repository
{
    //Servisler genericleştirilmez.
    public interface IGenericRepository<T>
    {
        Task<T> Add(T entity);
        Task<IReadOnlyList<T>> GetAll();
        Task<T?> GetById(int Id);
        Task Remove(T entity);
        Task Update(T entity);
        Task<bool> HasExist(int id);
    }
}
