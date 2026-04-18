using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Amenities.Commands.ActivateAmenity;

public class ActivateAmenityUseCase : IRequestHandler<ActivateAmenityCommand, bool>
{
    private readonly IAmenitiesRepository _amenitiesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateAmenityUseCase(IAmenitiesRepository amenitiesRepository, IUnitOfWork unitOfWork)
    {
        _amenitiesRepository = amenitiesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ActivateAmenityCommand command)
    {
        var amenity = await _amenitiesRepository.GetById(command.AmenityId);

        if (amenity is null)
            throw new NotFoundException("Amenity", command.AmenityId);
        
        amenity.SetAsActive();

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