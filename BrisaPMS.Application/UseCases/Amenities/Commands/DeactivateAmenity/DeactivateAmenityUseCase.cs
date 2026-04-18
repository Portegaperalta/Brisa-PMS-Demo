using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Amenities.Commands.DeactivateAmenity;

public class DeactivateAmenityUseCase : IRequestHandler<DeactivateAmenityCommand, bool>
{
    private readonly IAmenitiesRepository _amenitiesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateAmenityUseCase(IAmenitiesRepository amenitiesRepository, IUnitOfWork unitOfWork)
    {
        _amenitiesRepository = amenitiesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeactivateAmenityCommand command)
    {
        var amenity = await _amenitiesRepository.GetById(command.AmenityId);

        if (amenity is null)
            throw new NotFoundException("Amenity", command.AmenityId);

        amenity.SetAsInactive();

        try
        {
            await _amenitiesRepository.Update(amenity);
            await _unitOfWork.Persist();
            return true;
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}