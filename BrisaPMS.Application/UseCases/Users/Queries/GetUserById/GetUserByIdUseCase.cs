using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Users.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Users.Queries.GetUserById;

public class GetUserByIdUseCase : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUsersRepository _usersRepository;

    public GetUserByIdUseCase(IUsersRepository usersRepository) { _usersRepository = usersRepository; }

    public async Task<UserDto> Handle(GetUserByIdQuery query)
    {
        var user = await _usersRepository.GetById(query.UserId);
        
        if (user is null)
            throw new NotFoundException("User", query.UserId);

        return user.ToDto();
    }
}