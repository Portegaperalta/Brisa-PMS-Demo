using BrisaPMS.Domain.Users;

namespace BrisaPMS.Application.Contracts.Repositories;

public interface IUsersRepository : IRepository<User>
{
    Task<List<User>> GetAllByHotelIdAsync(Guid hotelId);
}