using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IBaseRepository<T> where T : class
    {
        IReadOnlyList<T> GetAll();
        void Add(T entity);
        void Remove(int Id);
        T? GetById(int Id);
        void Update(T entity);
    }
}
