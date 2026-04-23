using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.HouseKeeping;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.ChangeTaskDeadline;

public class ChangeTaskDeadlineUseCase : IRequestHandler<ChangeTaskDeadlineCommand, bool>
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeTaskDeadlineUseCase(IHouseKeepingTasksRepository houseKeepingTasksRepository, IUnitOfWork unitOfWork)
    {
        _houseKeepingTasksRepository = houseKeepingTasksRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ChangeTaskDeadlineCommand command)
    {
        var houseKeepingTask = await _houseKeepingTasksRepository.GetById(command.HouseKeepingTaskId);
        
        if (houseKeepingTask is null)
            throw new NotFoundException("HouseKeeping Task", command.HouseKeepingTaskId);
        
        var newTaskDeadline = new TaskDeadline(command.ExpectedStartTime, command.ExpectedEndTime);
        houseKeepingTask.ChangeTaskDeadline(newTaskDeadline);
        
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