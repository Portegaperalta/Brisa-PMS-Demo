using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Rooms.Commands.ChangeRoomType;
using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.RoomTypes;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Rooms.Commands.ChangeRoomType;

public class ChangeRoomUseCaseTests
{
  private readonly IRoomsRepository _roomsRepositoryMock;
  private readonly IRoomTypesRepository _roomTypesRepositoryMock;
  private readonly IUnitOfWork _unitOfWorkMock;
  private readonly ChangeRoomTypeUseCase _useCase;

  public ChangeRoomUseCaseTests()
  {
    _roomsRepositoryMock = Substitute.For<IRoomsRepository>();

    _roomTypesRepositoryMock = Substitute.For<IRoomTypesRepository>();

    _unitOfWorkMock = Substitute.For<IUnitOfWork>();

    _useCase = new ChangeRoomTypeUseCase(_roomsRepositoryMock, _roomTypesRepositoryMock, _unitOfWorkMock);
  }

  [Fact]
  public async Task Handle_ChangesRoomTypeAndReturnsTrue()
  {
    // Arrange
    var roomId = Guid.NewGuid();
    var roomTypeId = Guid.NewGuid();
    var command = CreateCommand(roomId, roomTypeId);
    var currentRoom = CreateRoom(roomId);
    var newRoomType = CreateRoomType(roomTypeId, "Presidential Suite");

    _roomsRepositoryMock.GetById(roomId).Returns(currentRoom);
    _roomTypesRepositoryMock.GetById(roomTypeId).Returns(newRoomType);

    // Act
    var result = await _useCase.Handle(command);

    // Assert
    await _roomsRepositoryMock.Received(1).GetById(roomId);
    await _roomTypesRepositoryMock.Received(1).GetById(roomTypeId);
    await _roomsRepositoryMock.Received(1).Update(Arg.Is<Room>(room =>
        room.Id == roomId &&
        room.RoomType == newRoomType));
    await _unitOfWorkMock.Received(1).Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();

    result.Should().BeTrue();
    currentRoom.RoomType.Should().BeSameAs(newRoomType);
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenRoomDoesNotExist()
  {
    // Arrange
    var command = CreateCommand(Guid.NewGuid(), Guid.NewGuid());

    _roomsRepositoryMock.GetById(command.RoomId).Returns((Room?)null);

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
    await _roomTypesRepositoryMock.DidNotReceive().GetById(Arg.Any<Guid>());
    await _roomsRepositoryMock.DidNotReceive().Update(Arg.Any<Room>());
    await _unitOfWorkMock.DidNotReceive().Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenRoomTypeDoesNotExist()
  {
    // Arrange
    var roomId = Guid.NewGuid();
    var roomTypeId = Guid.NewGuid();
    var command = CreateCommand(roomId, roomTypeId);

    _roomsRepositoryMock.GetById(roomId).Returns(CreateRoom(roomId));
    _roomTypesRepositoryMock.GetById(roomTypeId).Returns((RoomType?)null);

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
    await _roomsRepositoryMock.Received(1).GetById(roomId);
    await _roomTypesRepositoryMock.Received(1).GetById(roomTypeId);
    await _roomsRepositoryMock.DidNotReceive().Update(Arg.Any<Room>());
    await _unitOfWorkMock.DidNotReceive().Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();
  }

  [Fact]
  public async Task Handle_RevertsUnitOfWork_WhenRepositoryUpdateFails()
  {
    // Arrange
    var roomId = Guid.NewGuid();
    var roomTypeId = Guid.NewGuid();
    var command = CreateCommand(roomId, roomTypeId);
    var room = CreateRoom(roomId);
    var roomType = CreateRoomType(roomTypeId);

    _roomsRepositoryMock.GetById(roomId).Returns(room);
    _roomTypesRepositoryMock.GetById(roomTypeId).Returns(roomType);
    _roomsRepositoryMock.Update(Arg.Any<Room>()).Throws<InvalidOperationException>();

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<InvalidOperationException>();
    await _unitOfWorkMock.Received(1).Revert();
    await _unitOfWorkMock.DidNotReceive().Persist();
  }

  private static ChangeRoomTypeCommand CreateCommand(Guid roomId, Guid roomTypeId)
  {
    return new ChangeRoomTypeCommand
    {
      RoomId = roomId,
      RoomTypeId = roomTypeId
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

  private static RoomType CreateRoomType(Guid? roomTypeId = null, string name = "Deluxe Suite")
  {
    return new RoomType(
        name,
        25m,
        2,
        BedType.Queen,
        2,
        1,
        "Spacious suite with ocean view")
    {
      Id = roomTypeId ?? Guid.NewGuid()
    };
  }
}
