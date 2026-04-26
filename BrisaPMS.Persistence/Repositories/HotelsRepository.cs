using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.Hotels;

namespace BrisaPMS.Persistence.Repositories;

public class HotelsRepository : Repository<Hotel>, IHotelsRepository
{
    public HotelsRepository(BrisaPmsDbContext context) 
        :  base(context)
    {
        
    }
}