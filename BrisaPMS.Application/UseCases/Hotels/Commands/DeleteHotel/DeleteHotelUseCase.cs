using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.DeleteHotel;

public class DeleteHotelUseCase : IRequestHandler<DeleteHotelCommand, bool>
{
    private readonly IHotelsRepository _hotelsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteHotelUseCase(IHotelsRepository hotelsRepository, IUnitOfWork unitOfWork)
    {
        _hotelsRepository = hotelsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteHotelCommand command)
    {
        var hotel = await _hotelsRepository.GetById(command.Id) ??
                    throw new NotFoundException("Hotel", command.Id);

        try
        {
            await _hotelsRepository.Delete(hotel);
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