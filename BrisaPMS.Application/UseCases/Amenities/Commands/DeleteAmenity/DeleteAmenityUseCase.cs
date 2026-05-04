using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Amenities.Commands.DeleteAmenity;

public class DeleteAmenityUseCase : IRequestHandler<DeleteAmenityCommand, bool>
{
    private readonly IAmenitiesRepository _amenitiesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAmenityUseCase(IAmenitiesRepository amenitiesRepository, IUnitOfWork unitOfWork)
    {
        _amenitiesRepository = amenitiesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteAmenityCommand command)
    {
        var amenity = await _amenitiesRepository.GetById(command.Id);

        if (amenity is null)
            throw new NotFoundException("Amenity", command.Id);

        try
        {
            await _amenitiesRepository.Delete(amenity);
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