using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.RoomTypes;

namespace BrisaPMS.Persistence.Repositories
{
    public class RoomTypesRepository : Repository<RoomType>, IRoomTypesRepository
    {
        public RoomTypesRepository(BrisaPmsDbContext context) 
            : base(context)
        {
        }
    }
}
