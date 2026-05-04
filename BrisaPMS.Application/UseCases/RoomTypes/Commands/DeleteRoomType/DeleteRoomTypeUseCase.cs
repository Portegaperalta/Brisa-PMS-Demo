using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.DeleteRoomType;

public class DeleteRoomTypeUseCase : IRequestHandler<DeleteRoomTypeCommand, bool>
{
    private readonly IRoomTypesRepository _roomTypesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoomTypeUseCase(IRoomTypesRepository roomTypesRepository,
        IUnitOfWork unitOfWork)
    {
        _roomTypesRepository = roomTypesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteRoomTypeCommand command)
    {
        var roomType = await _roomTypesRepository.GetById(command.Id) ??
                       throw new NotFoundException("Room Type", command.Id);

        try
        {
            await _roomTypesRepository.Delete(roomType);
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