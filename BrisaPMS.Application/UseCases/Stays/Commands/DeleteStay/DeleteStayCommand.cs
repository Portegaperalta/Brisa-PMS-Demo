using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Stays.Commands.DeleteStay;

public class DeleteStayCommand : IRequest<bool>
{
    public required Guid Id { get; set; }
}