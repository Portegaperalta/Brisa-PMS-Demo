using BrisaPMS.Application.UseCases.Users.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Users.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<UserDto>
{
    public required Guid UserId { get; set; }
}