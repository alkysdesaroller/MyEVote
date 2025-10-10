using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using myEVote.Application.Interfaces.Repositorios;
using myEVote.Infraestructure.Persistence.Context;

namespace myEVote.Infraestructure.Persistence.Repositories;

public class GenericRepository<TEntity>(MyEVoteContext context) : IGenericRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _entity = context.Set<TEntity>();

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await _entity.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        _entity.Remove(entity);
        await context.SaveChangesAsync();
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _entity.AnyAsync(filter);
    }

    public virtual async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _entity.Where(filter).ToListAsync();
    }

    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        return await _entity.ToListAsync();
    }

    public virtual async Task<List<TEntity>> GetAllWithIncludeAsync(List<string> properties)
    {
        var query = _entity.AsQueryable();

        foreach (var property in properties)
        {
            query = query.Include(property);
        }

        return await query.ToListAsync();
    }

    public virtual async Task<TEntity> GetByIdAsync(int id)
    {
        return await _entity.FindAsync(id) ?? throw new InvalidOperationException();
    }

    public virtual async Task UpdateAsync(TEntity entity, int id)
    {
        _entity.Update(entity);
        await context.SaveChangesAsync();
    }
}