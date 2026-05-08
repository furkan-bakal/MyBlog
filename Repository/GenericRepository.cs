using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
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
    }
}
