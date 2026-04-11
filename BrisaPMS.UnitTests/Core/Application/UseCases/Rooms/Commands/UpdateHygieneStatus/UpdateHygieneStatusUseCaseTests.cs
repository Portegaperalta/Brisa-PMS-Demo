using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Rooms.Commands.UpdateHygieneStatus;
using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.RoomTypes;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Rooms.Commands.UpdateHygieneStatus;

public class UpdateHygieneStatusUseCaseTests
{
  private readonly IRoomsRepository _roomsRepositoryMock;
  private readonly IUnitOfWork _unitOfWorkMock;
  private readonly UpdateHygieneStatusUseCase _useCase;

  public UpdateHygieneStatusUseCaseTests()
  {
    _roomsRepositoryMock = Substitute.For<IRoomsRepository>();

    _unitOfWorkMock = Substitute.For<IUnitOfWork>();

    _useCase = new UpdateHygieneStatusUseCase(_roomsRepositoryMock, _unitOfWorkMock);
  }

  [Fact]
  public async Task Handle_UpdatesHygieneStatusAndReturnsTrue()
  {
    // Arrange
    var roomId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    var command = CreateCommand(roomId, userId, "Dirty");
    var room = CreateRoom(roomId);

    _roomsRepositoryMock.GetById(roomId).Returns(room);

    // Act
    var result = await _useCase.Handle(command);

    // Assert
    await _roomsRepositoryMock.Received(1).GetById(roomId);
    await _roomsRepositoryMock.Received(1).Update(Arg.Is<Room>(updatedRoom =>
        updatedRoom.Id == roomId &&
        updatedRoom.HygieneStatus == RoomHygieneStatus.Dirty));
    await _unitOfWorkMock.Received(1).Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();

    result.Should().BeTrue();
    room.HygieneStatus.Should().Be(RoomHygieneStatus.Dirty);
    room.LastCleanedAt.Should().BeNull();
    room.LastCleanedBy.Should().BeNull();
  }

  [Fact]
  public async Task Handle_UpdatesLastCleanedFields_WhenHygieneStatusIsClean()
  {
    // Arrange
    var roomId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    var command = CreateCommand(roomId, userId, "Clean");
    var room = CreateRoom(roomId, RoomAvailabilityStatus.Available, RoomHygieneStatus.Dirty);

    _roomsRepositoryMock.GetById(roomId).Returns(room);

    // Act
    var result = await _useCase.Handle(command);

    // Assert
    await _roomsRepositoryMock.Received(1).Update(Arg.Is<Room>(updatedRoom =>
        updatedRoom.Id == roomId &&
        updatedRoom.HygieneStatus == RoomHygieneStatus.Clean &&
        updatedRoom.LastCleanedBy == userId &&
        updatedRoom.LastCleanedAt.HasValue));
    await _unitOfWorkMock.Received(1).Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();

    result.Should().BeTrue();
    room.HygieneStatus.Should().Be(RoomHygieneStatus.Clean);
    room.LastCleanedBy.Should().Be(userId);
    room.LastCleanedAt.Should().NotBeNull();
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenRoomDoesNotExist()
  {
    // Arrange
    var command = CreateCommand(Guid.NewGuid(), Guid.NewGuid(), "Dirty");

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
    var userId = Guid.NewGuid();
    var command = CreateCommand(roomId, userId, "Dirty");
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

  private static UpdateHygieneStatusCommand CreateCommand(Guid roomId, Guid userId, string hygieneStatus)
  {
    return new UpdateHygieneStatusCommand
    {
      RoomId = roomId,
      UserId = userId,
      HygieneStatus = hygieneStatus
    };
  }

  private static Room CreateRoom(
      Guid? roomId = null,
      RoomAvailabilityStatus availabilityStatus = RoomAvailabilityStatus.Available,
      RoomHygieneStatus hygieneStatus = RoomHygieneStatus.Clean)
  {
    return new Room(
        Guid.NewGuid(),
        "101",
        1,
        availabilityStatus,
        hygieneStatus,
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
