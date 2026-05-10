using Core;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity<int>
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

        public async Task<IReadOnlyList<T>> GetAll()
        {
            var list = await DbSet.ToListAsync();
            return list.AsReadOnly();
        }

        public async Task<T?> GetById(int Id)
        {
            return await DbSet.FindAsync(Id);
        }

        public async Task Remove(T entity)
        {
            DbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public Task Update(T entity)
        {
            DbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task<bool> HasExist(int id)
        {
            return DbSet.AnyAsync(x => x.Id == id);
        }
    }
}
