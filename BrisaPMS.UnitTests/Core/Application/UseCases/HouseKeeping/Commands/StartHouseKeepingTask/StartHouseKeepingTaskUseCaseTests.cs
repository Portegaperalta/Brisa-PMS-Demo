using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.StartHouseKeepingTask;
using BrisaPMS.Domain.HouseKeeping;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.StartHouseKeepingTask;

public class StartHouseKeepingTaskUseCaseTests
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly StartHouseKeepingTaskUseCase _useCase;

    public StartHouseKeepingTaskUseCaseTests()
    {
        _houseKeepingTasksRepositoryMock = Substitute.For<IHouseKeepingTasksRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new StartHouseKeepingTaskUseCase(_houseKeepingTasksRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_StartsTaskAndReturnsTrue()
    {
        // Arrange
        var houseKeepingTaskId = Guid.NewGuid();
        var command = new StartHouseKeepingTaskCommand { HouseKeepingTaskId = houseKeepingTaskId };
        var houseKeepingTask = HouseKeepingCommandTestData.CreateHouseKeepingTask();

        _houseKeepingTasksRepositoryMock.GetById(houseKeepingTaskId).Returns(houseKeepingTask);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        result.Should().BeTrue();
        houseKeepingTask.Status.Should().Be(HouseKeepingTaskStatus.InProgress);
        houseKeepingTask.ActualTimeInterval.Should().NotBeNull();
        houseKeepingTask.ActualTimeInterval!.ActualStartAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        await _houseKeepingTasksRepositoryMock.Received(1).Update(houseKeepingTask);
        await _unitOfWorkMock.Received(1).Persist();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenTaskDoesNotExist()
    {
        // Arrange
        var command = new StartHouseKeepingTaskCommand { HouseKeepingTaskId = Guid.NewGuid() };

        _houseKeepingTasksRepositoryMock.GetById(command.HouseKeepingTaskId).Returns((HouseKeepingTask?)null);

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
        var command = new StartHouseKeepingTaskCommand { HouseKeepingTaskId = houseKeepingTaskId };
        var houseKeepingTask = HouseKeepingCommandTestData.CreateHouseKeepingTask();

        _houseKeepingTasksRepositoryMock.GetById(houseKeepingTaskId).Returns(houseKeepingTask);
        _houseKeepingTasksRepositoryMock.Update(Arg.Any<HouseKeepingTask>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }
}
