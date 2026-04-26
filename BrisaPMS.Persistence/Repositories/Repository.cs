using BrisaPMS.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BrisaPMS.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T: class
{
    protected readonly BrisaPmsDbContext Context;
    
    public Repository(BrisaPmsDbContext context)
    {
        Context = context;
    }
    
    public async Task<T?> GetById(Guid id)
    {
        return await Context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await Context.Set<T>().ToListAsync();
    }

    public Task<T> Create(T entity)
    {
        Context.Add(entity);
        return Task.FromResult(entity);
    }

    public Task Update(T entity)
    {
        Context.Update(entity);
        return Task.CompletedTask;
    }

    public Task Delete(T entity)
    {
        Context.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<bool> Exists(Guid id)
    {
        var entity = await Context.Set<T>().FindAsync(id);

        if (entity is null)
            return false;
        
        return true;
    }
}