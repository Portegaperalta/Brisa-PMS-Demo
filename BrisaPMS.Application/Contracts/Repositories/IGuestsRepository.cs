using BrisaPMS.Domain.Guests;

namespace BrisaPMS.Application.Contracts.Repositories;

public interface IGuestsRepository : IRepository<Guest>
{
    Task<Guest?> GetByHotelIdAsync(Guid hotelId);
    Task<List<Guest>> GetAllByHotelIdAsync(Guid hotelId);
}