using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Bookings.Commands.CreateBooking;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Bookings;
using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.RoomTypes;
using BrisaPMS.Domain.Shared.Exceptions;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Commands.CreateBooking;

public class CreateBookingUseCaseTests
{
    private readonly IBookingsRepository _bookingsRepositoryMock;
    private readonly IHotelsRepository _hotelsRepositoryMock;
    private readonly IGuestsRepository _guestsRepositoryMock;
    private readonly IRoomsRepository _roomsRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CreateBookingUseCase _useCase;

    public CreateBookingUseCaseTests()
    {
        _bookingsRepositoryMock = Substitute.For<IBookingsRepository>();
        _hotelsRepositoryMock = Substitute.For<IHotelsRepository>();
        _guestsRepositoryMock = Substitute.For<IGuestsRepository>();
        _roomsRepositoryMock = Substitute.For<IRoomsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new CreateBookingUseCase(
            _bookingsRepositoryMock,
            _hotelsRepositoryMock,
            _guestsRepositoryMock,
            _roomsRepositoryMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_CreatesBookingAndReturnsBookingId()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var guestId = Guid.NewGuid();
        var discountId = Guid.NewGuid();
        var command = CreateValidCommand(hotelId, roomId, guestId, discountId);
        var room = CreateRoom(hotelId, RoomAvailabilityStatus.Available);

        _hotelsRepositoryMock.Exists(hotelId).Returns(true);
        _guestsRepositoryMock.Exists(guestId).Returns(true);
        _roomsRepositoryMock.GetById(roomId).Returns(room);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        await _hotelsRepositoryMock.Received(1).Exists(hotelId);
        await _guestsRepositoryMock.Received(1).Exists(guestId);
        await _roomsRepositoryMock.Received(1).GetById(roomId);
        await _roomsRepositoryMock.Received(1).Update(Arg.Is<Room>(updatedRoom =>
            updatedRoom.HotelId == hotelId &&
            updatedRoom.AvailabilityStatus == RoomAvailabilityStatus.Reserved));
        await _bookingsRepositoryMock.Received(1).Create(Arg.Is<Booking>(booking =>
            booking.HotelId == command.HotelId &&
            booking.RoomId == command.RoomId &&
            booking.GuestId == command.GuestId &&
            booking.Source == BookingSource.Website &&
            booking.GuestCount.NumberOfAdults == command.NumberOfAdults &&
            booking.GuestCount.NumberOfChildren == command.NumberOfChildren &&
            booking.CheckInOutTimes.CheckInTime == command.CheckInTime &&
            booking.CheckInOutTimes.CheckOutTime == command.CheckOutTime &&
            booking.SpecialRequests == command.SpecialRequests &&
            booking.TotalPrice.Amount == command.TotalPrice &&
            booking.DiscountId == command.DiscountId));
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Handle_CreatesBooking_WhenOptionalFieldsAreNotProvided()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var guestId = Guid.NewGuid();
        var command = CreateCommand(
            hotelId,
            roomId,
            guestId,
            "Phone",
            1,
            0,
            new DateTime(2026, 4, 20, 15, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 4, 21, 11, 0, 0, DateTimeKind.Utc),
            null,
            180m,
            null);
        var room = CreateRoom(hotelId, RoomAvailabilityStatus.Available);

        _hotelsRepositoryMock.Exists(hotelId).Returns(true);
        _guestsRepositoryMock.Exists(guestId).Returns(true);
        _roomsRepositoryMock.GetById(roomId).Returns(room);

        // Act
        await _useCase.Handle(command);

        // Assert
        await _bookingsRepositoryMock.Received(1).Create(Arg.Is<Booking>(booking =>
            booking.SpecialRequests == null &&
            booking.DiscountId == null &&
            booking.Source == BookingSource.Phone));
        await _unitOfWorkMock.Received(1).Persist();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenHotelDoesNotExist()
    {
        // Arrange
        var command = CreateValidCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _hotelsRepositoryMock.Exists(command.HotelId).Returns(false);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _guestsRepositoryMock.DidNotReceive().Exists(Arg.Any<Guid>());
        await _roomsRepositoryMock.DidNotReceive().GetById(Arg.Any<Guid>());
        await _roomsRepositoryMock.DidNotReceive().Update(Arg.Any<Room>());
        await _bookingsRepositoryMock.DidNotReceive().Create(Arg.Any<Booking>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenGuestDoesNotExist()
    {
        // Arrange
        var command = CreateValidCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _hotelsRepositoryMock.Exists(command.HotelId).Returns(true);
        _guestsRepositoryMock.Exists(command.GuestId).Returns(false);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _roomsRepositoryMock.DidNotReceive().GetById(Arg.Any<Guid>());
        await _roomsRepositoryMock.DidNotReceive().Update(Arg.Any<Room>());
        await _bookingsRepositoryMock.DidNotReceive().Create(Arg.Any<Booking>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenRoomDoesNotExist()
    {
        // Arrange
        var command = CreateValidCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        _hotelsRepositoryMock.Exists(command.HotelId).Returns(true);
        _guestsRepositoryMock.Exists(command.GuestId).Returns(true);
        _roomsRepositoryMock.GetById(command.RoomId).Returns((Room?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _roomsRepositoryMock.DidNotReceive().Update(Arg.Any<Room>());
        await _bookingsRepositoryMock.DidNotReceive().Create(Arg.Any<Booking>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_ThrowsBusinessRuleException_WhenRoomDoesNotBelongToHotel()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var guestId = Guid.NewGuid();
        var command = CreateValidCommand(hotelId, roomId, guestId, Guid.NewGuid());
        var room = CreateRoom(Guid.NewGuid(), RoomAvailabilityStatus.Available);

        _hotelsRepositoryMock.Exists(hotelId).Returns(true);
        _guestsRepositoryMock.Exists(guestId).Returns(true);
        _roomsRepositoryMock.GetById(roomId).Returns(room);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage($"Room with ID: {command.RoomId} does not belong to hotel with ID: {command.HotelId}");
        await _roomsRepositoryMock.DidNotReceive().Update(Arg.Any<Room>());
        await _bookingsRepositoryMock.DidNotReceive().Create(Arg.Any<Booking>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Theory]
    [InlineData(RoomAvailabilityStatus.Reserved, "Room is reserved, unable to create booking")]
    [InlineData(RoomAvailabilityStatus.Occupied, "Room is occupied, unable to create booking")]
    [InlineData(RoomAvailabilityStatus.OutOfService, "Room is out of service, unable to create booking")]
    public async Task Handle_ThrowsBusinessRuleException_WhenRoomIsUnavailable(
        RoomAvailabilityStatus availabilityStatus,
        string expectedMessage)
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var guestId = Guid.NewGuid();
        var command = CreateValidCommand(hotelId, roomId, guestId, Guid.NewGuid());
        var room = CreateRoom(hotelId, availabilityStatus);

        _hotelsRepositoryMock.Exists(hotelId).Returns(true);
        _guestsRepositoryMock.Exists(guestId).Returns(true);
        _roomsRepositoryMock.GetById(roomId).Returns(room);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage(expectedMessage);
        await _roomsRepositoryMock.DidNotReceive().Update(Arg.Any<Room>());
        await _bookingsRepositoryMock.DidNotReceive().Create(Arg.Any<Booking>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenRepositoryCreateFails()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var guestId = Guid.NewGuid();
        var command = CreateValidCommand(hotelId, roomId, guestId, Guid.NewGuid());
        var room = CreateRoom(hotelId, RoomAvailabilityStatus.Available);

        _hotelsRepositoryMock.Exists(hotelId).Returns(true);
        _guestsRepositoryMock.Exists(guestId).Returns(true);
        _roomsRepositoryMock.GetById(roomId).Returns(room);
        _bookingsRepositoryMock.Create(Arg.Any<Booking>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _roomsRepositoryMock.Received(1).Update(Arg.Any<Room>());
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    private static CreateBookingCommand CreateValidCommand(Guid hotelId, Guid roomId, Guid guestId, Guid? discountId)
    {
        return CreateCommand(
            hotelId,
            roomId,
            guestId,
            "Website",
            2,
            1,
            new DateTime(2026, 4, 20, 15, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 4, 22, 11, 0, 0, DateTimeKind.Utc),
            "High floor please",
            250.75m,
            discountId);
    }

    private static CreateBookingCommand CreateCommand(
        Guid hotelId,
        Guid roomId,
        Guid guestId,
        string source,
        int numberOfAdults,
        int numberOfChildren,
        DateTime checkInTime,
        DateTime checkOutTime,
        string? specialRequests,
        decimal totalPrice,
        Guid? discountId)
    {
        return new CreateBookingCommand
        {
            HotelId = hotelId,
            RoomId = roomId,
            GuestId = guestId,
            Source = source,
            NumberOfAdults = numberOfAdults,
            NumberOfChildren = numberOfChildren,
            CheckInTime = checkInTime,
            CheckOutTime = checkOutTime,
            SpecialRequests = specialRequests,
            TotalPrice = totalPrice,
            DiscountId = discountId
        };
    }

    private static Room CreateRoom(Guid hotelId, RoomAvailabilityStatus availabilityStatus)
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
