using System.Linq.Expressions;

namespace myEVote.Application.Interfaces.Repositorios;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(int id);
    Task<List<TEntity>> GetAllAsync();
    Task<List<TEntity>> GetAllWithIncludeAsync(List<string> properties);
    Task<TEntity> AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity, int id);
    Task DeleteAsync(TEntity entity);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);
    Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter);
}