using Core;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity<Guid>
    {
        public DbSet<T> DbSet { get; set; }

        public GenericRepository(AppDbContext dbContext)
        {
            DbSet = dbContext.Set<T>();
        }
        public async Task<T> Add(T entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task<IReadOnlyList<T>> GetAll()
        {
            var list = await DbSet.ToListAsync();
            return list.AsReadOnly();
        }

        public async Task<T?> GetById(Guid Id)
        {
            return await DbSet.FindAsync(Id);
        }

        public async Task Remove(T entity)
        {
            entity.IsDeleted = true;
            entity.DeletedDate = DateTime.UtcNow;

            DbSet.Update(entity);
            await Task.CompletedTask;
        }

        public Task Update(T entity)
        {
            DbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task<bool> HasExist(Guid id)
        {
            return DbSet.AnyAsync(x => x.Id == id);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }
    }
}
