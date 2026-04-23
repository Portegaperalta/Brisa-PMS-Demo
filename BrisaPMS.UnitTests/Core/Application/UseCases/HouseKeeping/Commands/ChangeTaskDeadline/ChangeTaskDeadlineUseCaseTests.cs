using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.ChangeTaskDeadline;
using BrisaPMS.Domain.HouseKeeping;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.ChangeTaskDeadline;

public class ChangeTaskDeadlineUseCaseTests
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly ChangeTaskDeadlineUseCase _useCase;

    public ChangeTaskDeadlineUseCaseTests()
    {
        _houseKeepingTasksRepositoryMock = Substitute.For<IHouseKeepingTasksRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new ChangeTaskDeadlineUseCase(_houseKeepingTasksRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ChangesTaskDeadlineAndReturnsTrue()
    {
        // Arrange
        var houseKeepingTaskId = Guid.NewGuid();
        var newStartTime = new DateTime(2026, 4, 2, 10, 0, 0, DateTimeKind.Utc);
        var newEndTime = new DateTime(2026, 4, 2, 11, 0, 0, DateTimeKind.Utc);
        var command = new ChangeTaskDeadlineCommand
        {
            HouseKeepingTaskId = houseKeepingTaskId,
            ExpectedStartTime = newStartTime,
            ExpectedEndTime = newEndTime
        };
        var houseKeepingTask = HouseKeepingCommandTestData.CreateHouseKeepingTask();

        _houseKeepingTasksRepositoryMock.GetById(houseKeepingTaskId).Returns(houseKeepingTask);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        result.Should().BeTrue();
        houseKeepingTask.Deadline.ExpectedStartAt.Should().Be(newStartTime);
        houseKeepingTask.Deadline.ExpectedEndAt.Should().Be(newEndTime);
        await _houseKeepingTasksRepositoryMock.Received(1).Update(houseKeepingTask);
        await _unitOfWorkMock.Received(1).Persist();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenTaskDoesNotExist()
    {
        // Arrange
        var command = new ChangeTaskDeadlineCommand
        {
            HouseKeepingTaskId = Guid.NewGuid(),
            ExpectedStartTime = new DateTime(2026, 4, 2, 10, 0, 0, DateTimeKind.Utc),
            ExpectedEndTime = new DateTime(2026, 4, 2, 11, 0, 0, DateTimeKind.Utc)
        };

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
        var command = new ChangeTaskDeadlineCommand
        {
            HouseKeepingTaskId = houseKeepingTaskId,
            ExpectedStartTime = new DateTime(2026, 4, 2, 10, 0, 0, DateTimeKind.Utc),
            ExpectedEndTime = new DateTime(2026, 4, 2, 11, 0, 0, DateTimeKind.Utc)
        };
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
