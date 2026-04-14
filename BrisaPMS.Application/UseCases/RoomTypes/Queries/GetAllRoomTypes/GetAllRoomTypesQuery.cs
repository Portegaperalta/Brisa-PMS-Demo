using BrisaPMS.Application.UseCases.RoomTypes.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.RoomTypes.Queries.GetAllRoomTypes;

public class GetAllRoomTypesQuery : IRequest<List<RoomTypeDto>>
{
}