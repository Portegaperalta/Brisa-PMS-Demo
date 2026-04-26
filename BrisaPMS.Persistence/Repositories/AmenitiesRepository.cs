using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.Amenities;

namespace BrisaPMS.Persistence.Repositories;

public class AmenitiesRepository : Repository<Amenity>, IAmenitiesRepository
{
    public AmenitiesRepository(BrisaPmsDbContext context) 
        : base(context)
    {
        
    }
}