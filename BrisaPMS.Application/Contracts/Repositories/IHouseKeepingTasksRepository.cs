using BrisaPMS.Domain.HouseKeeping;

namespace BrisaPMS.Application.Contracts.Repositories;

public interface IHouseKeepingTasksRepository : IRepository<HouseKeepingTask>
{
    Task<IEnumerable<HouseKeepingTask>> GetByHotelIdAsync(Guid hotelId);
}