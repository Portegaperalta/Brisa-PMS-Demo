using BrisaPMS.Domain.Stays;

namespace BrisaPMS.Application.Contracts.Repositories;

public interface IStaysRepository : IRepository<Stay>
{
    Task<IEnumerable<Stay>> GetAllByHotelIdAsync(Guid hotelId);
    Task<IEnumerable<Stay>> GetAllByRoomIdAsync(Guid roomId);
    Task<IEnumerable<Stay>> GetAllByGuestIdAsync(Guid guestId);
}