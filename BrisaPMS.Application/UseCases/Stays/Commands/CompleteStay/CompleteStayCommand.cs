using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Stays.Commands.CompleteStay;

public class CompleteStayCommand : IRequest<bool>
{
    public required Guid StayId {get; set;}
}