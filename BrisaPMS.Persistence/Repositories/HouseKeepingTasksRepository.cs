using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.HouseKeeping;
using Microsoft.EntityFrameworkCore;

namespace BrisaPMS.Persistence.Repositories;

public class HouseKeepingTasksRepository : Repository<HouseKeepingTask>, IHouseKeepingTasksRepository
{
    public HouseKeepingTasksRepository(BrisaPmsDbContext context) 
        : base(context)
    {
    }

    public async Task<IEnumerable<HouseKeepingTask>> GetAllByHotelIdAsync(Guid hotelId)
    {
        return await Context.HouseKeepingTasks
            .Where(t => t.HotelId == hotelId)
            .ToListAsync();
    }

    public async Task<IEnumerable<HouseKeepingTask>> GetAllByRoomIdAsync(Guid roomId)
    {
        return await Context.HouseKeepingTasks
            .Where(t => t.RoomId == roomId)
            .ToListAsync();
    }
}