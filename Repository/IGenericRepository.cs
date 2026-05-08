using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
