using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Amenities.Commands.DeleteAmenity;

public class DeleteAmenityCommand : IRequest<bool>
{
    public required Guid Id { get; set; }
}