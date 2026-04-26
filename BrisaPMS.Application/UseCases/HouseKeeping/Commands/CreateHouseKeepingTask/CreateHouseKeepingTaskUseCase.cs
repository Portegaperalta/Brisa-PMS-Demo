using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.HouseKeeping;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.CreateHouseKeepingTask;

public class CreateHouseKeepingTaskUseCase : IRequestHandler<CreateHouseKeepingTaskCommand, Guid>
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepository;
    private readonly IHotelsRepository _hotelsRepository;
    private readonly IRoomsRepository _roomsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateHouseKeepingTaskUseCase(IHouseKeepingTasksRepository houseKeepingTasksRepository, 
        IHotelsRepository hotelsRepository,IRoomsRepository roomsRepository, IUsersRepository usersRepository,
        IUnitOfWork unitOfWork)
    {
        _houseKeepingTasksRepository = houseKeepingTasksRepository;
        _hotelsRepository = hotelsRepository;
        _roomsRepository = roomsRepository;
        _usersRepository = usersRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateHouseKeepingTaskCommand command)
    {
        var hotelExists = await _hotelsRepository.Exists(command.HotelId);
        
        if (hotelExists is not true)
            throw new NotFoundException("Hotel", command.HotelId);
        
        var assignedUserExists = await _usersRepository.Exists(command.AssignedTo);
        
        if (assignedUserExists is not true)
            throw new NotFoundException("User", command.AssignedTo);
        
        var roomExists = await _roomsRepository.Exists(command.RoomId);

        if (roomExists is not true)
            throw new NotFoundException("Room", command.RoomId);

        var houseKeepingTaskType = Enum.Parse<HouseKeepingTaskType>(command.HouseKeepingTaskType);
        var taskPriority = Enum.Parse<TaskPriority>(command.TaskPriority);
        var taskExpectedTimeInterval = new TaskDeadline(command.ExpectedStartTime, command.ExpectedEndTime);

        var houseKeepingTask = new HouseKeepingTask
        (
            command.HotelId,
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