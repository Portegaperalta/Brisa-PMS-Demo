using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Rooms.Commands.CreateRoom;
using BrisaPMS.Domain.Rooms;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Rooms.Commands.CreateRoom;

public class CreateRoomUseCaseTests
{
  private readonly IRoomsRepository _roomsRepositoryMock;
  private readonly IHotelsRepository _hotelsRepositoryMock;
  private readonly IRoomTypesRepository _roomTypesRepositoryMock;
  private readonly IUnitOfWork _unitOfWorkMock;
  private readonly CreateRoomUseCase _useCase;

  public CreateRoomUseCaseTests()
  {
    _roomsRepositoryMock = Substitute.For<IRoomsRepository>();

    _hotelsRepositoryMock = Substitute.For<IHotelsRepository>();

    _roomTypesRepositoryMock = Substitute.For<IRoomTypesRepository>();

    _unitOfWorkMock = Substitute.For<IUnitOfWork>();

    _useCase = new CreateRoomUseCase(
        _roomsRepositoryMock,
        _hotelsRepositoryMock,
        _roomTypesRepositoryMock,
        _unitOfWorkMock);
  }

  [Fact]
  public async Task Handle_CreatesRoomAndReturnsRoomId()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var roomTypeId = Guid.NewGuid();
    var command = CreateCommand(hotelId, roomTypeId);

    _hotelsRepositoryMock.Exists(hotelId).Returns(true);

    _roomTypesRepositoryMock.Exists(roomTypeId).Returns(true);

    _roomsRepositoryMock.Create(Arg.Any<Room>())
        .Returns(callInfo => callInfo.Arg<Room>());

    // Act
    var result = await _useCase.Handle(command);

    // Assert
    await _hotelsRepositoryMock.Received(1).Exists(hotelId);

    await _roomTypesRepositoryMock.Received(1).Exists(roomTypeId);

    await _roomsRepositoryMock.Received(1).Create(Arg.Is<Room>(room =>
        room.HotelId == hotelId &&
        room.RoomTypeId == roomTypeId &&
        room.Number == command.Number &&
        room.Floor == command.Floor &&
        room.AvailabilityStatus == RoomAvailabilityStatus.Available &&
        room.HygieneStatus == RoomHygieneStatus.Clean));

    await _unitOfWorkMock.Received(1).Persist();

    await _unitOfWorkMock.DidNotReceive().Revert();

    result.Should().NotBe(Guid.Empty);
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenHotelDoesNotExist()
  {
    // Arrange
    var command = CreateCommand(Guid.NewGuid(), Guid.NewGuid());

    _hotelsRepositoryMock.Exists(command.HotelId).Returns(false);

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
    await _roomTypesRepositoryMock.DidNotReceive().Exists(Arg.Any<Guid>());
    await _roomsRepositoryMock.DidNotReceive().Create(Arg.Any<Room>());
    await _unitOfWorkMock.DidNotReceive().Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenRoomTypeDoesNotExist()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var roomTypeId = Guid.NewGuid();
    var command = CreateCommand(hotelId, roomTypeId);

    _hotelsRepositoryMock.Exists(hotelId).Returns(true);
    _roomTypesRepositoryMock.Exists(roomTypeId).Returns(false);

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
    await _hotelsRepositoryMock.Received(1).Exists(hotelId);
    await _roomTypesRepositoryMock.Received(1).Exists(roomTypeId);
    await _roomsRepositoryMock.DidNotReceive().Create(Arg.Any<Room>());
    await _unitOfWorkMock.DidNotReceive().Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();
  }

  [Fact]
  public async Task Handle_RevertsUnitOfWork_WhenRepositoryCreateFails()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var roomTypeId = Guid.NewGuid();
    var command = CreateCommand(hotelId, roomTypeId);

    _hotelsRepositoryMock.Exists(hotelId).Returns(true);
    _roomTypesRepositoryMock.Exists(roomTypeId).Returns(true);
    _roomsRepositoryMock.Create(Arg.Any<Room>()).Throws<InvalidOperationException>();

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<InvalidOperationException>();
    await _unitOfWorkMock.Received(1).Revert();
    await _unitOfWorkMock.DidNotReceive().Persist();
  }

  private static CreateRoomCommand CreateCommand(Guid hotelId, Guid roomTypeId)
  {
    return new CreateRoomCommand
    {
      HotelId = hotelId,
      RoomTypeId = roomTypeId,
      Number = "101",
      Floor = 1,
      AvailabilityStatus = "Available",
      HygieneStatus = "Clean"
    };
  }

}
