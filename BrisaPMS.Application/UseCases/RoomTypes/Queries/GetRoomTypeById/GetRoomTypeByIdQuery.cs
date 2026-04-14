using BrisaPMS.Application.UseCases.RoomTypes.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.RoomTypes.Queries.GetRoomTypeById;

public class GetRoomTypeByIdQuery : IRequest<RoomTypeDto>
{
    public required Guid RoomTypeId { get; init; }
}