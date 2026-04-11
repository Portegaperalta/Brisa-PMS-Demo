using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Rooms.Commands.SetAsRestocked;
using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.RoomTypes;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Rooms.Commands.SetAsRestocked;

public class SetAsRestockedUseCaseTests
{
  private readonly IRoomsRepository _roomsRepositoryMock;
  private readonly IUnitOfWork _unitOfWorkMock;
  private readonly SetAsRestockedUseCase _useCase;

  public SetAsRestockedUseCaseTests()
  {
    _roomsRepositoryMock = Substitute.For<IRoomsRepository>();

    _unitOfWorkMock = Substitute.For<IUnitOfWork>();

    _useCase = new SetAsRestockedUseCase(_roomsRepositoryMock, _unitOfWorkMock);
  }

  [Fact]
  public async Task Handle_SetsRoomAsRestockedAndReturnsTrue()
  {
    // Arrange
    var roomId = Guid.NewGuid();
    var command = CreateCommand(roomId);
    var room = CreateRoom(roomId);
    room.SetAsPendingRestocking();

    _roomsRepositoryMock.GetById(roomId).Returns(room);

    // Act
    var result = await _useCase.Handle(command);

    // Assert
    await _roomsRepositoryMock.Received(1).GetById(roomId);
    await _roomsRepositoryMock.Received(1).Update(Arg.Is<Room>(updatedRoom =>
        updatedRoom.Id == roomId &&
        updatedRoom.NeedsRestocking == false));
    await _unitOfWorkMock.Received(1).Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();

    result.Should().BeTrue();
    room.NeedsRestocking.Should().BeFalse();
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenRoomDoesNotExist()
  {
    // Arrange
    var command = CreateCommand(Guid.NewGuid());

    _roomsRepositoryMock.GetById(command.RoomId).Returns((Room?)null);

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
    await _roomsRepositoryMock.DidNotReceive().Update(Arg.Any<Room>());
    await _unitOfWorkMock.DidNotReceive().Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();
  }

  [Fact]
  public async Task Handle_RevertsUnitOfWork_WhenRepositoryUpdateFails()
  {
    // Arrange
    var roomId = Guid.NewGuid();
    var command = CreateCommand(roomId);
    var room = CreateRoom(roomId);

    _roomsRepositoryMock.GetById(roomId).Returns(room);
    _roomsRepositoryMock.Update(Arg.Any<Room>()).Throws<InvalidOperationException>();

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<InvalidOperationException>();
    await _unitOfWorkMock.Received(1).Revert();
    await _unitOfWorkMock.DidNotReceive().Persist();
  }

  private static SetAsRestockedCommand CreateCommand(Guid roomId)
  {
    return new SetAsRestockedCommand
    {
      RoomId = roomId
    };
  }

  private static Room CreateRoom(Guid? roomId = null)
  {
    return new Room(
        Guid.NewGuid(),
        "101",
        1,
        RoomAvailabilityStatus.Available,
        RoomHygieneStatus.Clean,
        CreateRoomType())
    {
      Id = roomId ?? Guid.NewGuid()
    };
  }

  private static RoomType CreateRoomType()
  {
    return new RoomType(
        "Deluxe Suite",
        25m,
        2,
        BedType.Queen,
        2,
        1,
        "Spacious suite with ocean view");
  }
}
