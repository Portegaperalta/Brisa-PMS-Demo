using BrisaPMS.Application.UseCases.Users.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IRequest<List<UserDto>>
{
}