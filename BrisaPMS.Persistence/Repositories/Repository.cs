using BrisaPMS.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BrisaPMS.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T: class
{
    private readonly BrisaPmsDbContext _context;
    
    public Repository(BrisaPmsDbContext context)
    {
        _context = context;
    }
    
    public async Task<T?> GetById(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public Task<T> Create(T entity)
    {
        _context.Add(entity);
        return Task.FromResult(entity);
    }

    public Task Update(T entity)
    {
        _context.Update(entity);
        return Task.CompletedTask;
    }

    public Task Delete(T entity)
    {
        _context.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<bool> Exists(Guid id)
    {
        var entity = await _context.Set<T>().FindAsync(id);

        if (entity is null)
            return false;
        
        return true;
    }
}