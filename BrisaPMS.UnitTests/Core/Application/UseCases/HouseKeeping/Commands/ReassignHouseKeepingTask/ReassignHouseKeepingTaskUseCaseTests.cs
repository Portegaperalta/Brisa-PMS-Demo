using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.ReassignHouseKeepingTask;
using BrisaPMS.Domain.HouseKeeping;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.ReassignHouseKeepingTask;

public class ReassignHouseKeepingTaskUseCaseTests
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepositoryMock;
    private readonly IUsersRepository _usersRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly ReassignHouseKeepingTaskUseCase _useCase;

    public ReassignHouseKeepingTaskUseCaseTests()
    {
        _houseKeepingTasksRepositoryMock = Substitute.For<IHouseKeepingTasksRepository>();
        _usersRepositoryMock = Substitute.For<IUsersRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new ReassignHouseKeepingTaskUseCase(
            _houseKeepingTasksRepositoryMock,
            _usersRepositoryMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ReassignsTaskAndReturnsTrue()
    {
        // Arrange
        var houseKeepingTaskId = Guid.NewGuid();
        var newAssignedTo = Guid.NewGuid();
        var command = new ReassignHouseKeepingTaskCommand
        {
            HouseKeepingTaskId = houseKeepingTaskId,
            AssignedTo = newAssignedTo
        };
        var houseKeepingTask = HouseKeepingCommandTestData.CreateHouseKeepingTask();

        _houseKeepingTasksRepositoryMock.GetById(houseKeepingTaskId).Returns(houseKeepingTask);
        _usersRepositoryMock.Exists(newAssignedTo).Returns(true);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        result.Should().BeTrue();
        houseKeepingTask.AssignedTo.Should().Be(newAssignedTo);
        await _houseKeepingTasksRepositoryMock.Received(1).Update(houseKeepingTask);
        await _unitOfWorkMock.Received(1).Persist();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenTaskDoesNotExist()
    {
        // Arrange
        var command = new ReassignHouseKeepingTaskCommand
        {
            HouseKeepingTaskId = Guid.NewGuid(),
            AssignedTo = Guid.NewGuid()
        };

        _houseKeepingTasksRepositoryMock.GetById(command.HouseKeepingTaskId).Returns((HouseKeepingTask?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _usersRepositoryMock.DidNotReceive().Exists(Arg.Any<Guid>());
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenAssignedUserDoesNotExist()
    {
        // Arrange
        var houseKeepingTaskId = Guid.NewGuid();
        var command = new ReassignHouseKeepingTaskCommand
        {
            HouseKeepingTaskId = houseKeepingTaskId,
            AssignedTo = Guid.NewGuid()
        };
        var houseKeepingTask = HouseKeepingCommandTestData.CreateHouseKeepingTask();

        _houseKeepingTasksRepositoryMock.GetById(houseKeepingTaskId).Returns(houseKeepingTask);
        _usersRepositoryMock.Exists(command.AssignedTo).Returns(false);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _houseKeepingTasksRepositoryMock.DidNotReceive().Update(Arg.Any<HouseKeepingTask>());
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenUpdateFails()
    {
        // Arrange
        var houseKeepingTaskId = Guid.NewGuid();
        var command = new ReassignHouseKeepingTaskCommand
        {
            HouseKeepingTaskId = houseKeepingTaskId,
            AssignedTo = Guid.NewGuid()
        };
        var houseKeepingTask = HouseKeepingCommandTestData.CreateHouseKeepingTask();

        _houseKeepingTasksRepositoryMock.GetById(houseKeepingTaskId).Returns(houseKeepingTask);
        _usersRepositoryMock.Exists(command.AssignedTo).Returns(true);
        _houseKeepingTasksRepositoryMock.Update(Arg.Any<HouseKeepingTask>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }
}
