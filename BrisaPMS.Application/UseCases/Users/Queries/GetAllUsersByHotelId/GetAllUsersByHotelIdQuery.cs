using BrisaPMS.Application.UseCases.Users.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Users.Queries.GetAllUsersByHotelId;

public class GetAllUsersByHotelIdQuery : IRequest<List<UserDto>>
{
    public required Guid HotelId { get; set; }
}