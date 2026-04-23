using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.UpdateHouseKeepingTaskNotes;
using BrisaPMS.Domain.HouseKeeping;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands.UpdateHouseKeepingTaskNotes;

public class UpdateHouseKeepingTaskNotesUseCaseTests
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UpdateHouseKeepingTaskNotesUseCase _useCase;

    public UpdateHouseKeepingTaskNotesUseCaseTests()
    {
        _houseKeepingTasksRepositoryMock = Substitute.For<IHouseKeepingTasksRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new UpdateHouseKeepingTaskNotesUseCase(_houseKeepingTasksRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_UpdatesNotesAndReturnsTrue()
    {
        // Arrange
        var houseKeepingTaskId = Guid.NewGuid();
        var command = new UpdateHouseKeepingTaskNotesCommand
        {
            HouseKeepingTaskId = houseKeepingTaskId,
            Notes = "Replace towels and amenities"
        };
        var houseKeepingTask = HouseKeepingCommandTestData.CreateHouseKeepingTask();

        _houseKeepingTasksRepositoryMock.GetById(houseKeepingTaskId).Returns(houseKeepingTask);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        result.Should().BeTrue();
        houseKeepingTask.Notes.Should().Be(command.Notes);
        await _houseKeepingTasksRepositoryMock.Received(1).Update(houseKeepingTask);
        await _unitOfWorkMock.Received(1).Persist();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenTaskDoesNotExist()
    {
        // Arrange
        var command = new UpdateHouseKeepingTaskNotesCommand
        {
            HouseKeepingTaskId = Guid.NewGuid(),
            Notes = "Replace towels and amenities"
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
        var command = new UpdateHouseKeepingTaskNotesCommand
        {
            HouseKeepingTaskId = houseKeepingTaskId,
            Notes = "Replace towels and amenities"
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
