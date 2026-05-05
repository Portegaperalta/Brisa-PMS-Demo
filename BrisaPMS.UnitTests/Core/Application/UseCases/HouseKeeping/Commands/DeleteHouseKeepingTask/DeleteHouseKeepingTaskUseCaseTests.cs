using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.DeleteHouseKeepingTask;
using BrisaPMS.Domain.HouseKeeping;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.DeleteHouseKeepingTask;

public class DeleteHouseKeepingTaskUseCaseTests
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly DeleteHouseKeepingTaskUseCase _useCase;

    public DeleteHouseKeepingTaskUseCaseTests()
    {
        _houseKeepingTasksRepositoryMock = Substitute.For<IHouseKeepingTasksRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new DeleteHouseKeepingTaskUseCase(_houseKeepingTasksRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_DeletesHouseKeepingTask()
    {
        // Arrange
        var houseKeepingTask = HouseKeepingCommandTestData.CreateHouseKeepingTask();
        var command = new DeleteHouseKeepingTaskCommand { Id = houseKeepingTask.Id };

        _houseKeepingTasksRepositoryMock.GetById(command.Id).Returns(houseKeepingTask);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        await _houseKeepingTasksRepositoryMock.Received(1).Delete(houseKeepingTask);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenHouseKeepingTaskDoesNotExist()
    {
        // Arrange
        var command = new DeleteHouseKeepingTaskCommand { Id = Guid.NewGuid() };

        _houseKeepingTasksRepositoryMock.GetById(command.Id).Returns((HouseKeepingTask?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _houseKeepingTasksRepositoryMock.DidNotReceive().Delete(Arg.Any<HouseKeepingTask>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenRepositoryDeleteFails()
    {
        // Arrange
        var houseKeepingTask = HouseKeepingCommandTestData.CreateHouseKeepingTask();
        var command = new DeleteHouseKeepingTaskCommand { Id = houseKeepingTask.Id };

        _houseKeepingTasksRepositoryMock.GetById(command.Id).Returns(houseKeepingTask);
        _houseKeepingTasksRepositoryMock.Delete(Arg.Any<HouseKeepingTask>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }
}