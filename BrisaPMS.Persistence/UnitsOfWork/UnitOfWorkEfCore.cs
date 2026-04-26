using BrisaPMS.Application.Contracts.Persistence;

namespace BrisaPMS.Persistence.UnitsOfWork;

public class UnitOfWorkEfCore : IUnitOfWork
{
    private readonly BrisaPmsDbContext _context;
    
    public UnitOfWorkEfCore(BrisaPmsDbContext context)
    {
        _context = context;
    }
    
    public async Task Persist()
    {
        await _context.SaveChangesAsync();
    }

    public Task Revert()
    {
        return Task.CompletedTask;
    }
}