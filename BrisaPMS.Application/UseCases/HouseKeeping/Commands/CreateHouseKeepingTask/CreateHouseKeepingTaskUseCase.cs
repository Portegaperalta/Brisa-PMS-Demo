using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.HouseKeeping;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.CreateHouseKeepingTask;

public class CreateHouseKeepingTaskUseCase : IRequestHandler<CreateHouseKeepingTaskCommand, Guid>
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepository;
    private readonly IRoomsRepository _roomsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHouseKeepingTaskUseCase(IHouseKeepingTasksRepository houseKeepingTasksRepository
        ,IRoomsRepository roomsRepository, IUsersRepository usersRepository, IUnitOfWork unitOfWork)
    {
        _houseKeepingTasksRepository = houseKeepingTasksRepository;
        _roomsRepository = roomsRepository;
        _usersRepository = usersRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateHouseKeepingTaskCommand command)
    {
        var assignedUserExists = await _usersRepository.Exists(command.AssignedTo);
        
        if (assignedUserExists is not true)
            throw new NotFoundException("User", command.AssignedTo);
        
        var room = await _roomsRepository.GetById(command.RoomId);

        if (room is null)
            throw new NotFoundException("Room", command.RoomId);

        var houseKeepingTaskType = Enum.Parse<HouseKeepingTaskType>(command.HouseKeepingTaskType);
        var taskPriority = Enum.Parse<TaskPriority>(command.TaskPriority);
        var taskExpectedTimeInterval = new TaskDeadline(command.ExpectedStartTime, command.ExpectedEndTime);

        var houseKeepingTask = new HouseKeepingTask
        (
            room.HotelId,
            command.RoomId,
            command.AssignedTo,
            command.AssignedBy,
            houseKeepingTaskType,
            taskPriority,
            taskExpectedTimeInterval,
            command.Notes
        );

        try
        {
            await _houseKeepingTasksRepository.Create(houseKeepingTask);
            await _unitOfWork.Persist();
            return houseKeepingTask.Id;
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}