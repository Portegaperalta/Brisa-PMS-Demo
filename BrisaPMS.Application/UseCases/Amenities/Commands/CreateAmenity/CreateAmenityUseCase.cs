using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.Amenities.Shared;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Amenities;

namespace BrisaPMS.Application.UseCases.Amenities.Commands.CreateAmenity;

public class CreateAmenityUseCase : IRequestHandler<CreateAmenityCommand, AmenityDto>
{
    private readonly IAmenitiesRepository _amenitiesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAmenityUseCase(IAmenitiesRepository amenitiesRepository, IUnitOfWork unitOfWork)
    {
        _amenitiesRepository = amenitiesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AmenityDto> Handle(CreateAmenityCommand command)
    {
        var amenity = new Amenity(command.Name, command.Description, command.IsActive);

        try
        {
            await _amenitiesRepository.Create(amenity);
            await _unitOfWork.Persist();
            return amenity.ToDto();
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}