using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Stays.Commands.CreateStay;

public class CreateStayCommand : IRequest<Guid>
{
    public required Guid GuestId { get; set; }
    public required Guid BookingId { get; set; }
}