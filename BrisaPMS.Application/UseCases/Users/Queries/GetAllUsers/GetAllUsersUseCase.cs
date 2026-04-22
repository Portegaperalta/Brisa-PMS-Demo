using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.Users.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Users.Queries.GetAllUsers;

public class GetAllUsersUseCase : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    private readonly IUsersRepository _usersRepository;

    public GetAllUsersUseCase(IUsersRepository usersRepository) { _usersRepository = usersRepository; }

    public async Task<List<UserDto>> Handle(GetAllUsersQuery query)
    {
        var users = await _usersRepository.GetAll();
        var usersDtos = users.Select(u => u.ToDto()).ToList();
        return usersDtos;
    }
}