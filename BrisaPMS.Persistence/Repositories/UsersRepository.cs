using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace BrisaPMS.Persistence.Repositories;

public class UsersRepository : Repository<User>, IUsersRepository
{
    public UsersRepository(BrisaPmsDbContext context) 
        : base(context)
    {
    }

    public async Task<List<User>> GetAllByHotelIdAsync(Guid hotelId)
    {
        return await Context.Users
            .Where(u => u.HotelId == hotelId)
            .ToListAsync();
    }
}