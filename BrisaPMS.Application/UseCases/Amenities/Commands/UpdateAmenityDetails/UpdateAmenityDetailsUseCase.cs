using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Amenities.Commands.UpdateAmenityDetails;

public class UpdateAmenityDetailsUseCase : IRequestHandler<UpdateAmenityDetailsCommand, bool>
{
    private readonly IAmenitiesRepository _amenitiesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAmenityDetailsUseCase(IAmenitiesRepository amenitiesRepository, IUnitOfWork unitOfWork)
    {
        _amenitiesRepository = amenitiesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateAmenityDetailsCommand command)
    {
        var amenity = await _amenitiesRepository.GetById(command.AmenityId);

        if (amenity is null)
            throw new NotFoundException("Amenity", command.AmenityId);
        
        amenity.UpdateName(command.Name);
        amenity.UpdateDescription(command.Description);

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