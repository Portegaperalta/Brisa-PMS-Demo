using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.UpdateHouseKeepingTaskNotes;

public class UpdateHouseKeepingTaskNotesUseCase : IRequestHandler<UpdateHouseKeepingTaskNotesCommand, bool>
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateHouseKeepingTaskNotesUseCase(IHouseKeepingTasksRepository houseKeepingTasksRepository, 
        IUnitOfWork unitOfWork)
    {
        _houseKeepingTasksRepository = houseKeepingTasksRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateHouseKeepingTaskNotesCommand command)
    {
        var houseKeepingTask = await _houseKeepingTasksRepository.GetById(command.HouseKeepingTaskId);
        
        if (houseKeepingTask is null)
            throw new NotFoundException("HouseKeeping Task", command.HouseKeepingTaskId);
        
        houseKeepingTask.UpdatedNotes(command.Notes);
        
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