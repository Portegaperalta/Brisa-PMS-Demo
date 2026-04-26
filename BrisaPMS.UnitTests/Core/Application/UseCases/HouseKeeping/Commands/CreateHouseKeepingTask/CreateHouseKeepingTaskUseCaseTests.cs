using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.CreateHouseKeepingTask;
using BrisaPMS.Domain.HouseKeeping;
using BrisaPMS.Domain.Rooms;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.CreateHouseKeepingTask;

public class CreateHouseKeepingTaskUseCaseTests
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepositoryMock;
    private readonly IRoomsRepository _roomsRepositoryMock;
    private readonly IUsersRepository _usersRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CreateHouseKeepingTaskUseCase _useCase;

    public CreateHouseKeepingTaskUseCaseTests()
    {
        _houseKeepingTasksRepositoryMock = Substitute.For<IHouseKeepingTasksRepository>();
        _roomsRepositoryMock = Substitute.For<IRoomsRepository>();
        _usersRepositoryMock = Substitute.For<IUsersRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new CreateHouseKeepingTaskUseCase(
            _houseKeepingTasksRepositoryMock,
            _roomsRepositoryMock,
            _usersRepositoryMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_CreatesTaskAndReturnsTaskId()
    {
        // Arrange
        var room = HouseKeepingCommandTestData.CreateRoom();
        var assignedTo = Guid.NewGuid();
        var assignedBy = Guid.NewGuid();
        var command = new CreateHouseKeepingTaskCommand
        {
            RoomId = room.Id,
            AssignedTo = assignedTo,
            AssignedBy = assignedBy,
            HouseKeepingTaskType = nameof(HouseKeepingTaskType.Cleaning),
            TaskPriority = nameof(TaskPriority.High),
            ExpectedStartTime = new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc),
            ExpectedEndTime = new DateTime(2026, 4, 1, 11, 0, 0, DateTimeKind.Utc),
            Notes = "Clean room before next guest arrival"
        };

        _usersRepositoryMock.Exists(assignedTo).Returns(true);
        _roomsRepositoryMock.GetById(command.RoomId).Returns(room);
        _houseKeepingTasksRepositoryMock.Create(Arg.Any<HouseKeepingTask>())
            .Returns(callInfo => callInfo.Arg<HouseKeepingTask>());

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        result.Should().NotBe(Guid.Empty);
        await _houseKeepingTasksRepositoryMock.Received(1).Create(Arg.Is<HouseKeepingTask>(task =>
            task.HotelId == room.HotelId &&
            task.RoomId == room.Id &&
            task.AssignedTo == assignedTo &&
            task.AssignedBy == assignedBy &&
            task.Type == HouseKeepingTaskType.Cleaning &&
            task.Priority == TaskPriority.High &&
            task.Notes == command.Notes));
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenAssignedUserDoesNotExist()
    {
        // Arrange
        var command = CreateCommand();

        _usersRepositoryMock.Exists(command.AssignedTo).Returns(false);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _roomsRepositoryMock.DidNotReceive().GetById(Arg.Any<Guid>());
        await _houseKeepingTasksRepositoryMock.DidNotReceive().Create(Arg.Any<HouseKeepingTask>());
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenRoomDoesNotExist()
    {
        // Arrange
        var command = CreateCommand();

        _usersRepositoryMock.Exists(command.AssignedTo).Returns(true);
        _roomsRepositoryMock.GetById(command.RoomId).Returns((Room?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _houseKeepingTasksRepositoryMock.DidNotReceive().Create(Arg.Any<HouseKeepingTask>());
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenCreateFails()
    {
        // Arrange
        var command = CreateCommand();
        var room = HouseKeepingCommandTestData.CreateRoom(roomId: command.RoomId);

        _usersRepositoryMock.Exists(command.AssignedTo).Returns(true);
        _roomsRepositoryMock.GetById(command.RoomId).Returns(room);
        _houseKeepingTasksRepositoryMock.Create(Arg.Any<HouseKeepingTask>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    private static CreateHouseKeepingTaskCommand CreateCommand()
    {
        return new CreateHouseKeepingTaskCommand
        {
            RoomId = Guid.NewGuid(),
            AssignedTo = Guid.NewGuid(),
            AssignedBy = Guid.NewGuid(),
            HouseKeepingTaskType = nameof(HouseKeepingTaskType.Cleaning),
            TaskPriority = nameof(TaskPriority.High),
            ExpectedStartTime = new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc),
            ExpectedEndTime = new DateTime(2026, 4, 1, 11, 0, 0, DateTimeKind.Utc),
            Notes = "Clean room before next guest arrival"
        };
    }
}
