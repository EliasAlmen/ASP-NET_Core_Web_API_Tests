using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Diagnostics;
using System.Linq.Expressions;

namespace WebApi.Repositories
{
    public interface IRepo<TEntity, TDbContext> where TEntity : class where TDbContext : DbContext
    {
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> ReadAsync(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> ReadAllAsync();
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
    }


    public abstract class Repo<TEntity, TDbContext> : IRepo<TEntity, TDbContext> where TEntity : class where TDbContext : DbContext
    {

        private readonly TDbContext _dbContext;

        protected Repo(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                return await _dbContext.Set<TEntity>().AnyAsync(expression);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            try
            {
                await _dbContext.Set<TEntity>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<TEntity> ReadAsync(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
               return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(expression) ?? null!;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<IEnumerable<TEntity>> ReadAllAsync()
        {
            try
            {
                return await _dbContext.Set<TEntity>().ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
