using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.HouseKeeping;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.StartHouseKeepingTask;

public class StartHouseKeepingTaskUseCase : IRequestHandler<StartHouseKeepingTaskCommand, bool>
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StartHouseKeepingTaskUseCase(IHouseKeepingTasksRepository houseKeepingTasksRepository, IUnitOfWork unitOfWork)
    {
        _houseKeepingTasksRepository = houseKeepingTasksRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(StartHouseKeepingTaskCommand command)
    {
        var houseKeepingTask = await _houseKeepingTasksRepository.GetById(command.HouseKeepingTaskId);
        
        if (houseKeepingTask is null)
            throw new NotFoundException("HouseKeeping Task", command.HouseKeepingTaskId);
        
        houseKeepingTask.UpdatedStatus(HouseKeepingTaskStatus.InProgress);
        houseKeepingTask.StartActualTimeInterval();
        
        try
        {
            await _houseKeepingTasksRepository.Update(houseKeepingTask);
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