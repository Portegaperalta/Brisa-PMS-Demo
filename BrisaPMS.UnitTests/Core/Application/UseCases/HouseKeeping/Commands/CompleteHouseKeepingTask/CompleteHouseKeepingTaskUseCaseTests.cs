using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.CompleteHouseKeepingTask;
using BrisaPMS.Domain.HouseKeeping;
using BrisaPMS.Domain.Rooms;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.CompleteHouseKeepingTask;

public class CompleteHouseKeepingTaskUseCaseTests
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepositoryMock;
    private readonly IRoomsRepository _roomsRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CompleteHouseKeepingTaskUseCase _useCase;

    public CompleteHouseKeepingTaskUseCaseTests()
    {
        _houseKeepingTasksRepositoryMock = Substitute.For<IHouseKeepingTasksRepository>();
        _roomsRepositoryMock = Substitute.For<IRoomsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new CompleteHouseKeepingTaskUseCase(
            _houseKeepingTasksRepositoryMock,
            _roomsRepositoryMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_CompletesTaskAndMarksRoomAsClean()
    {
        // Arrange
        var houseKeepingTaskId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var command = new CompleteHouseKeepingTaskCommand { HouseKeepingTaskId = houseKeepingTaskId };
        var houseKeepingTask = HouseKeepingCommandTestData.CreateHouseKeepingTask(
            roomId: roomId,
            type: HouseKeepingTaskType.Cleaning,
            startActualTimeInterval: true);
        var room = HouseKeepingCommandTestData.CreateRoom(roomId: roomId, hygieneStatus: RoomHygieneStatus.Dirty);

        _houseKeepingTasksRepositoryMock.GetById(houseKeepingTaskId).Returns(houseKeepingTask);
        _roomsRepositoryMock.GetById(roomId).Returns(room);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        result.Should().BeTrue();
        houseKeepingTask.Status.Should().Be(HouseKeepingTaskStatus.Completed);
        houseKeepingTask.ActualTimeInterval.Should().NotBeNull();
        houseKeepingTask.ActualTimeInterval!.ActualEndAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        room.HygieneStatus.Should().Be(RoomHygieneStatus.Clean);
        await _houseKeepingTasksRepositoryMock.Received(1).Update(houseKeepingTask);
        await _roomsRepositoryMock.Received(1).Update(room);
        await _unitOfWorkMock.Received(1).Persist();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenTaskDoesNotExist()
    {
        // Arrange
        var command = new CompleteHouseKeepingTaskCommand { HouseKeepingTaskId = Guid.NewGuid() };

        _houseKeepingTasksRepositoryMock.GetById(command.HouseKeepingTaskId).Returns((HouseKeepingTask?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _roomsRepositoryMock.DidNotReceive().GetById(Arg.Any<Guid>());
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenRoomDoesNotExist()
    {
        // Arrange
        var houseKeepingTaskId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var command = new CompleteHouseKeepingTaskCommand { HouseKeepingTaskId = houseKeepingTaskId };
        var houseKeepingTask = HouseKeepingCommandTestData.CreateHouseKeepingTask(roomId: roomId, startActualTimeInterval: true);

        _houseKeepingTasksRepositoryMock.GetById(houseKeepingTaskId).Returns(houseKeepingTask);
        _roomsRepositoryMock.GetById(roomId).Returns((Room?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _houseKeepingTasksRepositoryMock.DidNotReceive().Update(Arg.Any<HouseKeepingTask>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenUpdateFails()
    {
        // Arrange
        var houseKeepingTaskId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var command = new CompleteHouseKeepingTaskCommand { HouseKeepingTaskId = houseKeepingTaskId };
        var houseKeepingTask = HouseKeepingCommandTestData.CreateHouseKeepingTask(
            roomId: roomId,
            startActualTimeInterval: true);
        var room = HouseKeepingCommandTestData.CreateRoom(roomId: roomId);

        _houseKeepingTasksRepositoryMock.GetById(houseKeepingTaskId).Returns(houseKeepingTask);
        _roomsRepositoryMock.GetById(roomId).Returns(room);
        _houseKeepingTasksRepositoryMock.Update(Arg.Any<HouseKeepingTask>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }
}
