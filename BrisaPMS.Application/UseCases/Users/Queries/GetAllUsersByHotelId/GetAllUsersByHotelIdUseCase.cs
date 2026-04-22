using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Users.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Users.Queries.GetAllUsersByHotelId;

public class GetAllUsersByHotelIdUseCase : IRequestHandler<GetAllUsersByHotelIdQuery, List<UserDto>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IHotelsRepository _hotelsRepository;

    public GetAllUsersByHotelIdUseCase(IUsersRepository usersRepository, IHotelsRepository hotelsRepository)
    {
        _usersRepository = usersRepository;
        _hotelsRepository = hotelsRepository;
    }

    public async Task<List<UserDto>> Handle(GetAllUsersByHotelIdQuery query)
    {
        var hotelExists = await _hotelsRepository.Exists(query.HotelId);

        if (hotelExists is not true)
            throw new NotFoundException("Hotel", query.HotelId);

        var users = await _usersRepository.GetAllByHotelIdAsync(query.HotelId);
        var usersDtos = users.Select(u => u.ToDto()).ToList();
        return usersDtos;
    }
}