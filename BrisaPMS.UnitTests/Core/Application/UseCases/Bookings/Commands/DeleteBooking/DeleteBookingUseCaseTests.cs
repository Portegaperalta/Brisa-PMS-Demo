using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Bookings.Commands.DeleteBooking;
using BrisaPMS.Domain.Bookings;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Commands.DeleteBooking;

public class DeleteBookingUseCaseTests
{
  private readonly IBookingsRepository _bookingsRepositoryMock;
  private readonly IUnitOfWork _unitOfWorkMock;
  private readonly DeleteBookingUseCase _useCase;

  public DeleteBookingUseCaseTests()
  {
    _bookingsRepositoryMock = Substitute.For<IBookingsRepository>();
    _unitOfWorkMock = Substitute.For<IUnitOfWork>();
    _useCase = new DeleteBookingUseCase(_bookingsRepositoryMock, _unitOfWorkMock);
  }

  [Fact]
  public async Task Handle_DeletesBooking()
  {
    // Arrange
    var bookingId = Guid.NewGuid();
    var booking = CreateBooking(bookingId);
    var command = new DeleteBookingCommand { Id = bookingId };

    _bookingsRepositoryMock.GetById(bookingId).Returns(booking);

    // Act
    var result = await _useCase.Handle(command);

    // Assert
    await _bookingsRepositoryMock.Received(1).Delete(booking);
    await _unitOfWorkMock.Received(1).Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();
    result.Should().BeTrue();
  }

  [Fact]
  public async Task Handle_ThrowsNotFoundException_WhenBookingDoesNotExist()
  {
    // Arrange
    var command = new DeleteBookingCommand { Id = Guid.NewGuid() };

    _bookingsRepositoryMock.GetById(command.Id).Returns((Booking?)null);

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<NotFoundException>();
    await _bookingsRepositoryMock.DidNotReceive().Delete(Arg.Any<Booking>());
    await _unitOfWorkMock.DidNotReceive().Persist();
    await _unitOfWorkMock.DidNotReceive().Revert();
  }

  [Fact]
  public async Task Handle_RevertsUnitOfWork_WhenRepositoryDeleteFails()
  {
    // Arrange
    var bookingId = Guid.NewGuid();
    var booking = CreateBooking(bookingId);
    var command = new DeleteBookingCommand { Id = bookingId };

    _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
    _bookingsRepositoryMock.Delete(Arg.Any<Booking>()).Throws<InvalidOperationException>();

    // Act
    var act = async () => await _useCase.Handle(command);

    // Assert
    await act.Should().ThrowAsync<InvalidOperationException>();
    await _unitOfWorkMock.Received(1).Revert();
    await _unitOfWorkMock.DidNotReceive().Persist();
  }

  private static Booking CreateBooking(Guid? bookingId = null)
  {
    return new Booking(
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        BookingSource.Website,
        new GuestCount(2, 1),
        new CheckInOutTimes(new DateTime(2026, 4, 20, 15, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 22, 11, 0, 0, DateTimeKind.Utc)),
        new Money(250.75m, CurrencyCode.USD))
    {
      Id = bookingId ?? Guid.NewGuid()
    };
  }
}