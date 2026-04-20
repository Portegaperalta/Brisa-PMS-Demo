using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Bookings.Queries.GetBookingById;
using BrisaPMS.Application.UseCases.Bookings.Queries.Shared;
using BrisaPMS.Domain.Bookings;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Queries.GetBookingById;

public class GetBookingByIdUseCaseTests
{
    private readonly IBookingsRepository _bookingsRepositoryMock;
    private readonly GetBookingByIdUseCase _useCase;

    public GetBookingByIdUseCaseTests()
    {
        _bookingsRepositoryMock = Substitute.For<IBookingsRepository>();
        _useCase = new GetBookingByIdUseCase(_bookingsRepositoryMock);
    }

    [Fact]
    public async Task Handle_ReturnsBookingDto()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var booking = CreateBooking(bookingId);
        var query = new GetBookingByIdQuery { BookingId = bookingId };

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<BookingDto>();
        result.Id.Should().Be(booking.Id);
        result.HotelId.Should().Be(booking.HotelId);
        result.RoomId.Should().Be(booking.RoomId);
        result.GuestId.Should().Be(booking.GuestId);
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenBookingDoesNotExist()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var query = new GetBookingByIdQuery { BookingId = bookingId };

        _bookingsRepositoryMock.GetById(bookingId).ReturnsNull();

        // Act
        var act = async () => await _useCase.Handle(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_CallsBookingsRepository()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        var booking = CreateBooking(bookingId);
        var query = new GetBookingByIdQuery { BookingId = bookingId };

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);

        // Act
        await _useCase.Handle(query);

        // Assert
        await _bookingsRepositoryMock.Received(1).GetById(bookingId);
    }

    private static Booking CreateBooking(Guid? bookingId = null)
    {
        var booking = new Booking(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            BookingSource.Website,
            new GuestCount(2, 1),
            new CheckInOutTimes(new DateTime(2026, 4, 20, 15, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 22, 11, 0, 0, DateTimeKind.Utc)),
            new Money(250.75m, CurrencyCode.USD));

        if (bookingId.HasValue)
        {
            typeof(Booking).GetProperty("Id")!.SetValue(booking, bookingId.Value);
        }

        return booking;
    }
}