using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.ReassignHouseKeepingTask;

public class ReassignHouseKeepingTaskUseCase : IRequestHandler<ReassignHouseKeepingTaskCommand, bool>
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReassignHouseKeepingTaskUseCase(IHouseKeepingTasksRepository houseKeepingTasksRepository,
        IUsersRepository usersRepository, IUnitOfWork unitOfWork)
    {
        _houseKeepingTasksRepository = houseKeepingTasksRepository;
        _usersRepository = usersRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ReassignHouseKeepingTaskCommand command)
    {
        var houseKeepingTask = await _houseKeepingTasksRepository.GetById(command.HouseKeepingTaskId);

        if (houseKeepingTask is null)
            throw new NotFoundException("HouseKeeping Task", command.HouseKeepingTaskId);

        var newAssignedUserExists = await _usersRepository.Exists(command.AssignedTo);

        if (newAssignedUserExists is not true)
            throw new NotFoundException("User", command.AssignedTo);
        
        houseKeepingTask.ChangeAssignedTo(command.AssignedTo);

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