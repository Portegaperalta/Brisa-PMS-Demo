using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.HouseKeeping;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.ChangeHouseKeepingTaskType;

public class ChangeHouseKeepingTaskTypeUseCase : IRequestHandler<ChangeHouseKeepingTaskTypeCommand, bool>
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeHouseKeepingTaskTypeUseCase(IHouseKeepingTasksRepository houseKeepingTasksRepository,
        IUnitOfWork unitOfWork)
    {
        _houseKeepingTasksRepository = houseKeepingTasksRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ChangeHouseKeepingTaskTypeCommand command)
    {
        var houseKeepingTask = await _houseKeepingTasksRepository.GetById(command.HouseKeepingTaskId);
        
        if (houseKeepingTask is null)
            throw new NotFoundException("HouseKeeping Task", command.HouseKeepingTaskId);

        var newHouseKeepingTaskType = Enum.Parse<HouseKeepingTaskType>(command.HouseKeepingTaskType);
        houseKeepingTask.ChangeTaskType(newHouseKeepingTaskType);
        
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