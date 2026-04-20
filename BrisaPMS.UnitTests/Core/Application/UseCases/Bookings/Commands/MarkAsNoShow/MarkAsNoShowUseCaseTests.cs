using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Bookings.Commands.MarkAsNoShow;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Booking;
using BrisaPMS.Domain.Bookings;
using BrisaPMS.Domain.RoomTypes;
using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Commands.MarkAsNoShow;

public class MarkAsNoShowUseCaseTests
{
    private readonly IBookingsRepository _bookingsRepositoryMock;
    private readonly IRoomsRepository _roomsRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly MarkAsNoShowUseCase _useCase;

    public MarkAsNoShowUseCaseTests()
    {
        _bookingsRepositoryMock = Substitute.For<IBookingsRepository>();
        _roomsRepositoryMock = Substitute.For<IRoomsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new MarkAsNoShowUseCase(_bookingsRepositoryMock, _roomsRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_MarksBookingAsNoShowAndReturnsTrue()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId);
        var booking = CreateBooking(hotelId, roomId);
        var room = CreateRoom(hotelId, roomId, RoomAvailabilityStatus.Reserved);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
        _roomsRepositoryMock.GetById(roomId).Returns(room);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        booking.Status.Should().Be(BookingStatus.NoShow);
        result.Should().Be(true);
    }

    [Fact]
    public async Task Handle_UpdatesRoomAvailabilityToAvailable()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId);
        var booking = CreateBooking(hotelId, roomId);
        var room = CreateRoom(hotelId, roomId, RoomAvailabilityStatus.Reserved);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
        _roomsRepositoryMock.GetById(roomId).Returns(room);

        // Act
        await _useCase.Handle(command);

        // Assert
        room.AvailabilityStatus.Should().Be(RoomAvailabilityStatus.Available);
    }

    [Fact]
    public async Task Handle_CallsBookingsRepository()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId);
        var booking = CreateBooking(hotelId, roomId);
        var room = CreateRoom(hotelId, roomId, RoomAvailabilityStatus.Reserved);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
        _roomsRepositoryMock.GetById(roomId).Returns(room);

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
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId);
        var booking = CreateBooking(hotelId, roomId);
        var room = CreateRoom(hotelId, roomId, RoomAvailabilityStatus.Reserved);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
        _roomsRepositoryMock.GetById(roomId).Returns(room);

        // Act
        await _useCase.Handle(command);

        // Assert
        await _roomsRepositoryMock.Received(1).GetById(roomId);
        await _roomsRepositoryMock.Received(1).Update(Arg.Any<Room>());
    }

    [Fact]
    public async Task Handle_CallsUnitOfWorkPersist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId);
        var booking = CreateBooking(hotelId, roomId);
        var room = CreateRoom(hotelId, roomId, RoomAvailabilityStatus.Reserved);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
        _roomsRepositoryMock.GetById(roomId).Returns(room);

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
        var command = CreateValidCommand(bookingId);

        _bookingsRepositoryMock.GetById(bookingId).Returns((Booking?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();

        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_ThrowsBusinessRuleException_WhenBookingIsAlreadyNoShow()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId);
        var booking = CreateBooking(hotelId, roomId, BookingStatus.NoShow);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Booking is already set as No Show");
    }

    [Fact]
    public async Task Handle_ThrowsBusinessRuleException_WhenBookingIsAlreadyCompleted()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId);
        var booking = CreateBooking(hotelId, roomId, BookingStatus.Complete);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Completed booking can't be set as No Show");
    }

    [Fact]
    public async Task Handle_ThrowsBusinessRuleException_WhenBookingIsAlreadyCancelled()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId);
        var booking = CreateBooking(hotelId, roomId, BookingStatus.Cancelled);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Cancelled booking can't be set as No Show");
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenUpdateFails()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId);
        var booking = CreateBooking(hotelId, roomId);
        var room = CreateRoom(hotelId, roomId, RoomAvailabilityStatus.Reserved);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
        _roomsRepositoryMock.GetById(roomId).Returns(room);
        _bookingsRepositoryMock.Update(Arg.Any<Booking>()).Throws<InvalidOperationException>();

        // Act
        await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.Handle(command));

        // Assert
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    private static MarkAsNoShowCommand CreateValidCommand(Guid bookingId)
    {
        return new MarkAsNoShowCommand
        {
            BookingId = bookingId
        };
    }

    private static Booking CreateBooking(Guid hotelId, Guid roomId, BookingStatus status = BookingStatus.Confirmed)
    {
        var booking = new Booking(
            hotelId,
            roomId,
            Guid.NewGuid(),
            BookingSource.Website,
            new GuestCount(2, 1),
            new CheckInOutTimes(new DateTime(2026, 4, 20, 15, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 22, 11, 0, 0, DateTimeKind.Utc)),
            new Money(250.75m, CurrencyCode.USD));

        typeof(Booking).GetProperty("Status")!.SetValue(booking, status);
        return booking;
    }

    private static Room CreateRoom(Guid hotelId, Guid roomId, RoomAvailabilityStatus availabilityStatus)
    {
        return new Room(
            hotelId,
            "101",
            1,
            availabilityStatus,
            RoomHygieneStatus.Clean,
            CreateRoomType());
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