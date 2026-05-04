using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.DeleteHotel;

public class DeleteHotelCommand : IRequest<bool>
{
    public required Guid Id { get; set; }
}