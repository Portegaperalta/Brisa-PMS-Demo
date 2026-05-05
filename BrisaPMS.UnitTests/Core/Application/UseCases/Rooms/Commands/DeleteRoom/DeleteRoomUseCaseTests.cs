using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Rooms.Commands.DeleteRoom;
using BrisaPMS.Domain.Rooms;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Rooms.Commands.DeleteRoom;

public class DeleteRoomUseCaseTests
{
    private readonly IRoomsRepository _roomsRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly DeleteRoomUseCase _useCase;

    public DeleteRoomUseCaseTests()
    {
        _roomsRepositoryMock = Substitute.For<IRoomsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new DeleteRoomUseCase(_roomsRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_DeletesRoom()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var room = CreateRoom(roomId);
        var command = new DeleteRoomCommand { Id = roomId };

        _roomsRepositoryMock.GetById(roomId).Returns(room);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        await _roomsRepositoryMock.Received(1).Delete(room);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenRoomDoesNotExist()
    {
        // Arrange
        var command = new DeleteRoomCommand { Id = Guid.NewGuid() };

        _roomsRepositoryMock.GetById(command.Id).Returns((Room?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _roomsRepositoryMock.DidNotReceive().Delete(Arg.Any<Room>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenRepositoryDeleteFails()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var room = CreateRoom(roomId);
        var command = new DeleteRoomCommand { Id = roomId };

        _roomsRepositoryMock.GetById(roomId).Returns(room);
        _roomsRepositoryMock.Delete(Arg.Any<Room>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    private static Room CreateRoom(Guid? roomId = null)
    {
        return new Room(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "101",
            1,
            RoomAvailabilityStatus.Available,
            RoomHygieneStatus.Clean)
        {
            Id = roomId ?? Guid.NewGuid()
        };
    }
}
