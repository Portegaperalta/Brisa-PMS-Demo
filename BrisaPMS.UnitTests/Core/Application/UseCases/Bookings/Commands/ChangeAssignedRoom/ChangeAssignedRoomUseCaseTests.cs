using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Bookings.Commands.ChangeAssignedRoom;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Bookings;
using BrisaPMS.Domain.RoomTypes;
using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Commands.ChangeAssignedRoom;

public class ChangeAssignedRoomUseCaseTests
{
  private readonly IBookingsRepository _bookingsRepositoryMock;
  private readonly IRoomsRepository _roomsRepositoryMock;
  private readonly IUnitOfWork _unitOfWorkMock;
  private readonly ChangeAssignedRoomUseCase _useCase;

  public ChangeAssignedRoomUseCaseTests()
  {
    _bookingsRepositoryMock = Substitute.For<IBookingsRepository>();
    _roomsRepositoryMock = Substitute.For<IRoomsRepository>();
    _unitOfWorkMock = Substitute.For<IUnitOfWork>();
    _useCase = new ChangeAssignedRoomUseCase(_bookingsRepositoryMock, _roomsRepositoryMock, _unitOfWorkMock);
  }

  [Fact]
  public async Task Handle_ChangesAssignedRoomAndReturnsTrue()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var bookingId = Guid.NewGuid();
    var currentRoomId = Guid.NewGuid();
    var newRoomId = Guid.NewGuid();

    var command = CreateValidCommand(bookingId, newRoomId);
    var booking = CreateBooking(hotelId, currentRoomId);
    var currentRoom = CreateRoom(hotelId, currentRoomId, RoomAvailabilityStatus.Reserved);
    var newRoom = CreateRoom(hotelId, newRoomId, RoomAvailabilityStatus.Available);

    _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
    _roomsRepositoryMock.GetById(currentRoomId).Returns(currentRoom);
    _roomsRepositoryMock.GetById(newRoomId).Returns(newRoom);

    // Act
    var result = await _useCase.Handle(command);

    // Assert
    booking.RoomId.Should().Be(newRoomId);
    result.Should().Be(true);
  }

  [Fact]
  public async Task Handle_UpdatesCurrentRoomAvailabilityToAvailable()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var bookingId = Guid.NewGuid();
    var currentRoomId = Guid.NewGuid();
    var newRoomId = Guid.NewGuid();
    var command = CreateValidCommand(bookingId, newRoomId);
    var booking = CreateBooking(hotelId, currentRoomId);
    var currentRoom = CreateRoom(hotelId, currentRoomId, RoomAvailabilityStatus.Reserved);
    var newRoom = CreateRoom(hotelId, newRoomId, RoomAvailabilityStatus.Available);

    _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
    _roomsRepositoryMock.GetById(currentRoomId).Returns(currentRoom);
    _roomsRepositoryMock.GetById(newRoomId).Returns(newRoom);

    // Act
    await _useCase.Handle(command);

    // Assert
    currentRoom.AvailabilityStatus.Should().Be(RoomAvailabilityStatus.Available);
  }

  [Fact]
  public async Task Handle_UpdatesNewRoomAvailabilityToReserved()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var bookingId = Guid.NewGuid();
    var currentRoomId = Guid.NewGuid();
    var newRoomId = Guid.NewGuid();
    var command = CreateValidCommand(bookingId, newRoomId);
    var booking = CreateBooking(hotelId, currentRoomId);
    var currentRoom = CreateRoom(hotelId, currentRoomId, RoomAvailabilityStatus.Reserved);
    var newRoom = CreateRoom(hotelId, newRoomId, RoomAvailabilityStatus.Available);

    _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
    _roomsRepositoryMock.GetById(currentRoomId).Returns(currentRoom);
    _roomsRepositoryMock.GetById(newRoomId).Returns(newRoom);

    // Act
    await _useCase.Handle(command);

    // Assert
    newRoom.AvailabilityStatus.Should().Be(RoomAvailabilityStatus.Reserved);
  }

  [Fact]
  public async Task Handle_CallsBookingsRepository()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var bookingId = Guid.NewGuid();
    var currentRoomId = Guid.NewGuid();
    var newRoomId = Guid.NewGuid();
    var command = CreateValidCommand(bookingId, newRoomId);
    var booking = CreateBooking(hotelId, currentRoomId);
    var currentRoom = CreateRoom(hotelId, currentRoomId, RoomAvailabilityStatus.Reserved);
    var newRoom = CreateRoom(hotelId, newRoomId, RoomAvailabilityStatus.Available);

    _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
    _roomsRepositoryMock.GetById(currentRoomId).Returns(currentRoom);
    _roomsRepositoryMock.GetById(newRoomId).Returns(newRoom);

    // Act
    await _useCase.Handle(command);

    // Assert
    await _bookingsRepositoryMock.Received(1).GetById(bookingId);
    await _bookingsRepositoryMock.Received(1).Update(Arg.Any<Booking>());
  }

  [Fact]
  public async Task Handle_CallsRoomsRepository()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var bookingId = Guid.NewGuid();
    var currentRoomId = Guid.NewGuid();
    var newRoomId = Guid.NewGuid();
    var command = CreateValidCommand(bookingId, newRoomId);
    var booking = CreateBooking(hotelId, currentRoomId);
    var currentRoom = CreateRoom(hotelId, currentRoomId, RoomAvailabilityStatus.Reserved);
    var newRoom = CreateRoom(hotelId, newRoomId, RoomAvailabilityStatus.Available);

    _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
    _roomsRepositoryMock.GetById(currentRoomId).Returns(currentRoom);
    _roomsRepositoryMock.GetById(newRoomId).Returns(newRoom);

    // Act
    await _useCase.Handle(command);

    // Assert
    await _roomsRepositoryMock.Received(1).GetById(currentRoomId);
    await _roomsRepositoryMock.Received(1).GetById(newRoomId);
    await _roomsRepositoryMock.Received(2).Update(Arg.Any<Room>());
  }

  [Fact]
  public async Task Handle_CallsUnitOfWorkPersist()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var bookingId = Guid.NewGuid();
    var currentRoomId = Guid.NewGuid();
    var newRoomId = Guid.NewGuid();
    var command = CreateValidCommand(bookingId, newRoomId);
    var booking = CreateBooking(hotelId, currentRoomId);
    var currentRoom = CreateRoom(hotelId, currentRoomId, RoomAvailabilityStatus.Reserved);
    var newRoom = CreateRoom(hotelId, newRoomId, RoomAvailabilityStatus.Available);

    _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
    _roomsRepositoryMock.GetById(currentRoomId).Returns(currentRoom);
    _roomsRepositoryMock.GetById(newRoomId).Returns(newRoom);

    // Act
    await _useCase.Handle(command);

    // Assert
    await _unitOfWorkMock.Received(1).Persist();
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenBookingDoesNotExist()
  {
    // Arrange
    var bookingId = Guid.NewGuid();
    var newRoomId = Guid.NewGuid();
    var command = CreateValidCommand(bookingId, newRoomId);

    _bookingsRepositoryMock.GetById(bookingId).Returns((Booking?)null);

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();

    await _unitOfWorkMock.DidNotReceive().Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenNewRoomDoesNotExist()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var bookingId = Guid.NewGuid();
    var currentRoomId = Guid.NewGuid();
    var newRoomId = Guid.NewGuid();
    var command = CreateValidCommand(bookingId, newRoomId);
    var booking = CreateBooking(hotelId, currentRoomId);
    var currentRoom = CreateRoom(hotelId, currentRoomId, RoomAvailabilityStatus.Reserved);

    _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
    _roomsRepositoryMock.GetById(currentRoomId).Returns(currentRoom);
    _roomsRepositoryMock.GetById(newRoomId).Returns((Room?)null);

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task Handle_ThrowsBusinessRuleException_WhenNewRoomIsReserved()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var bookingId = Guid.NewGuid();
    var currentRoomId = Guid.NewGuid();
    var newRoomId = Guid.NewGuid();
    var command = CreateValidCommand(bookingId, newRoomId);
    var booking = CreateBooking(hotelId, currentRoomId);
    var currentRoom = CreateRoom(hotelId, currentRoomId, RoomAvailabilityStatus.Reserved);
    var newRoom = CreateRoom(hotelId, newRoomId, RoomAvailabilityStatus.Reserved);

    _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
    _roomsRepositoryMock.GetById(currentRoomId).Returns(currentRoom);
    _roomsRepositoryMock.GetById(newRoomId).Returns(newRoom);

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<BusinessRuleException>()
        .WithMessage("Requested room is already reserved");
  }

  [Fact]
  public async Task Handle_ThrowsBusinessRuleException_WhenNewRoomIsOccupied()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var bookingId = Guid.NewGuid();
    var currentRoomId = Guid.NewGuid();
    var newRoomId = Guid.NewGuid();
    var command = CreateValidCommand(bookingId, newRoomId);
    var booking = CreateBooking(hotelId, currentRoomId);
    var currentRoom = CreateRoom(hotelId, currentRoomId, RoomAvailabilityStatus.Reserved);
    var newRoom = CreateRoom(hotelId, newRoomId, RoomAvailabilityStatus.Occupied);

    _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
    _roomsRepositoryMock.GetById(currentRoomId).Returns(currentRoom);
    _roomsRepositoryMock.GetById(newRoomId).Returns(newRoom);

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<BusinessRuleException>()
        .WithMessage("Requested room is currently occupied");
  }

  [Fact]
  public async Task Handle_ThrowsBusinessRuleException_WhenNewRoomIsOutOfService()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var bookingId = Guid.NewGuid();
    var currentRoomId = Guid.NewGuid();
    var newRoomId = Guid.NewGuid();
    var command = CreateValidCommand(bookingId, newRoomId);
    var booking = CreateBooking(hotelId, currentRoomId);
    var currentRoom = CreateRoom(hotelId, currentRoomId, RoomAvailabilityStatus.Reserved);
    var newRoom = CreateRoom(hotelId, newRoomId, RoomAvailabilityStatus.OutOfService);

    _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
    _roomsRepositoryMock.GetById(currentRoomId).Returns(currentRoom);
    _roomsRepositoryMock.GetById(newRoomId).Returns(newRoom);

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<BusinessRuleException>()
        .WithMessage("Requested room is currently out of service");
  }

  [Fact]
  public async Task Handle_RevertsUnitOfWork_WhenUpdateFails()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var bookingId = Guid.NewGuid();
    var currentRoomId = Guid.NewGuid();
    var newRoomId = Guid.NewGuid();
    var command = CreateValidCommand(bookingId, newRoomId);
    var booking = CreateBooking(hotelId, currentRoomId);
    var currentRoom = CreateRoom(hotelId, currentRoomId, RoomAvailabilityStatus.Reserved);
    var newRoom = CreateRoom(hotelId, newRoomId, RoomAvailabilityStatus.Available);

    _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
    _roomsRepositoryMock.GetById(currentRoomId).Returns(currentRoom);
    _roomsRepositoryMock.GetById(newRoomId).Returns(newRoom);
    _bookingsRepositoryMock.Update(Arg.Any<Booking>()).Throws<InvalidOperationException>();

    // Act
    await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.Handle(command));

    // Assert
    await _unitOfWorkMock.Received(1).Revert();
    await _unitOfWorkMock.DidNotReceive().Persist();
  }

  private static ChangeAssignedRoomCommand CreateValidCommand(Guid bookingId, Guid roomId)
  {
    return new ChangeAssignedRoomCommand
    {
      BookingId = bookingId,
      RoomId = roomId
    };
  }

  private static Booking CreateBooking(Guid hotelId, Guid roomId)
  {
    return new Booking(
        hotelId,
        roomId,
        Guid.NewGuid(),
        BookingSource.Website,
        new GuestCount(2, 1),
        new CheckInOutTimes(new DateTime(2026, 4, 20, 15, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 22, 11, 0, 0, DateTimeKind.Utc)),
        new Money(250.75m, CurrencyCode.USD));
  }

  private static Room CreateRoom(Guid hotelId, Guid roomId, RoomAvailabilityStatus availabilityStatus)
  {
    return new Room(
        Guid.NewGuid(),
        hotelId,
        "101",
        1,
        availabilityStatus,
        RoomHygieneStatus.Clean);
  }

  private static RoomType CreateRoomType()
  {
    return new RoomType(
        "Deluxe Suite",
        new RoomBaseRate(0.25m),
        new RoomBed(BedType.Double, 1),
        new OccupancyPolicy(2, 1),
        "Ocean view suite");
  }
}
