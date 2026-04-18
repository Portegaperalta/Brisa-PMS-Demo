using BrisaPMS.Application.UseCases.Guests.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Queries.GetGuestById;

public class GetGuestByIdQuery : IRequest<GuestDto>
{
    public required Guid GuestId { get; set; }
}