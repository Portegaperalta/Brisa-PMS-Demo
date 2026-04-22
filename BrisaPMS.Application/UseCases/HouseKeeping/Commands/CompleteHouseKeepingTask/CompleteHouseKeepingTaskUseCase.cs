using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.HouseKeeping;
using BrisaPMS.Domain.Rooms;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.CompleteHouseKeepingTask;

public class CompleteHouseKeepingTaskUseCase : IRequestHandler<CompleteHouseKeepingTaskCommand, bool>
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepository;
    private readonly IRoomsRepository _roomsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteHouseKeepingTaskUseCase(IHouseKeepingTasksRepository houseKeepingTasksRepository,
        IRoomsRepository roomsRepository ,IUnitOfWork unitOfWork)
    {
        _houseKeepingTasksRepository = houseKeepingTasksRepository;
        _roomsRepository = roomsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CompleteHouseKeepingTaskCommand command)
    {
        var houseKeepingTask = await _houseKeepingTasksRepository.GetById(command.HouseKeepingTaskId);
        
        if (houseKeepingTask is null)
            throw new NotFoundException("HouseKeeping Task", command.HouseKeepingTaskId);
        
        var room = await _roomsRepository.GetById(houseKeepingTask.RoomId);
        
        var actualTimeInterval = new TaskActualTimeInterval(houseKeepingTask.ActualTimeInterval!.ActualStartAt, DateTime.UtcNow);
        
        houseKeepingTask.UpdatedStatus(HouseKeepingTaskStatus.Completed);
        houseKeepingTask.EndActualTimeInterval(actualTimeInterval);
        room!.UpdateHygieneStatus(RoomHygieneStatus.Clean);
        
        try
        {
            await _houseKeepingTasksRepository.Update(houseKeepingTask);
            await _roomsRepository.Update(room);
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