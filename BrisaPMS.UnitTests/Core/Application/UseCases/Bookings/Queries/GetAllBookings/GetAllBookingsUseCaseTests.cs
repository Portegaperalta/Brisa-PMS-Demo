using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.Bookings.Queries.GetAllBookings;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Bookings;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Queries.GetAllBookings;

public class GetAllBookingsUseCaseTests
{
  private readonly IBookingsRepository _bookingsRepositoryMock;
  private readonly GetAllBookingsUseCase _useCase;

  public GetAllBookingsUseCaseTests()
  {
    _bookingsRepositoryMock = Substitute.For<IBookingsRepository>();
    _useCase = new GetAllBookingsUseCase(_bookingsRepositoryMock);
  }

  [Fact]
  public async Task Handle_ReturnsListOfBookingDtos()
  {
    // Arrange
    var bookings = new List<Booking>
    {
      CreateBooking("101"),
      CreateBooking("102"),
      CreateBooking("103")
    };

    var query = new GetAllBookingsQuery();

    _bookingsRepositoryMock.GetAll().Returns(bookings);

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
    var bookings = new List<Booking>();
    var query = new GetAllBookingsQuery();

    _bookingsRepositoryMock.GetAll().Returns(bookings);

    // Act
    var result = await _useCase.Handle(query);

    // Assert
    result.Should().NotBeNull();
    result.Should().BeEmpty();
  }

  [Fact]
  public async Task Handle_CallsBookingsRepository()
  {
    // Arrange
    var bookings = new List<Booking> { CreateBooking("101") };
    var query = new GetAllBookingsQuery();

    _bookingsRepositoryMock.GetAll().Returns(bookings);

    // Act
    await _useCase.Handle(query);

    // Assert
    await _bookingsRepositoryMock.Received(1).GetAll();
  }

  private static Booking CreateBooking(string roomNumber)
  {
    return new Booking(
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        BookingSource.Website,
        new GuestCount(2, 1),
        new CheckInOutTimes(new DateTime(2026, 4, 20, 15, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 22, 11, 0, 0, DateTimeKind.Utc)),
        new Money(250.75m, CurrencyCode.USD));
  }
}