using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Bookings.Queries.GetBookingsByHotelId;
using BrisaPMS.Domain.Bookings;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Queries.GetBookingsByHotelId;

public class GetBookingsByHotelIdUseCaseTests
{
  private readonly IBookingsRepository _bookingsRepositoryMock;
  private readonly IHotelsRepository _hotelsRepositoryMock;
  private readonly GetBookingsByHotelIdUseCase _useCase;

  public GetBookingsByHotelIdUseCaseTests()
  {
    _bookingsRepositoryMock = Substitute.For<IBookingsRepository>();
    _hotelsRepositoryMock = Substitute.For<IHotelsRepository>();
    _useCase = new GetBookingsByHotelIdUseCase(_bookingsRepositoryMock, _hotelsRepositoryMock);
  }

  [Fact]
  public async Task Handle_ReturnsListOfBookingDtos_WhenHotelExists()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var bookings = new List<Booking>
    {
      CreateBooking(hotelId, "101"),
      CreateBooking(hotelId, "102"),
      CreateBooking(hotelId, "103")
    };

    var query = new GetBookingsByHotelIdQuery { HotelId = hotelId };

    _hotelsRepositoryMock.Exists(hotelId).Returns(true);
    _bookingsRepositoryMock.GetAllByHotelIdAsync(hotelId).Returns(bookings);

    // Act
    var result = await _useCase.Handle(query);

    // Assert
    result.Should().NotBeNull();
    result.Should().HaveCount(3);
  }

  [Fact]
  public async Task Handle_ReturnsEmptyList_WhenNoBookingsExist()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var bookings = new List<Booking>();
    var query = new GetBookingsByHotelIdQuery { HotelId = hotelId };

    _hotelsRepositoryMock.Exists(hotelId).Returns(true);
    _bookingsRepositoryMock.GetAllByHotelIdAsync(hotelId).Returns(bookings);

    // Act
    var result = await _useCase.Handle(query);

    // Assert
    result.Should().NotBeNull();
    result.Should().BeEmpty();
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenHotelDoesNotExist()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var query = new GetBookingsByHotelIdQuery { HotelId = hotelId };

    _hotelsRepositoryMock.Exists(hotelId).Returns(false);

    // Act
    var act = async () => await _useCase.Handle(query);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
  }

  [Fact]
  public async Task Handle_CallsBothRepositories()
  {
    // Arrange
    var hotelId = Guid.NewGuid();
    var bookings = new List<Booking> { CreateBooking(hotelId, "101") };
    var query = new GetBookingsByHotelIdQuery { HotelId = hotelId };

    _hotelsRepositoryMock.Exists(hotelId).Returns(true);
    _bookingsRepositoryMock.GetAllByHotelIdAsync(hotelId).Returns(bookings);

    // Act
    await _useCase.Handle(query);

    // Assert
    await _hotelsRepositoryMock.Received(1).Exists(hotelId);
    await _bookingsRepositoryMock.Received(1).GetAllByHotelIdAsync(hotelId);
  }

  private static Booking CreateBooking(Guid hotelId, string roomNumber)
  {
    return new Booking(
        hotelId,
        Guid.NewGuid(),
        Guid.NewGuid(),
        BookingSource.Website,
        new GuestCount(2, 1),
        new CheckInOutTimes(new DateTime(2026, 4, 20, 15, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 22, 11, 0, 0, DateTimeKind.Utc)),
        new Money(250.75m, CurrencyCode.USD));
  }
}