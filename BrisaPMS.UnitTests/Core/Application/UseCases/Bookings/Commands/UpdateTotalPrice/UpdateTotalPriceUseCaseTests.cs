using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Bookings.Commands.UpdateTotalPrice;
using BrisaPMS.Domain.Booking;
using BrisaPMS.Domain.Bookings;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Commands.UpdateTotalPrice;

public class UpdateTotalPriceUseCaseTests
{
    private readonly IBookingsRepository _bookingsRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UpdateTotalPriceUseCase _useCase;

    public UpdateTotalPriceUseCaseTests()
    {
        _bookingsRepositoryMock = Substitute.For<IBookingsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new UpdateTotalPriceUseCase(_bookingsRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_UpdatesTotalPriceAndReturnsTrue()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId, 350.50m);
        var booking = CreateBooking(hotelId, roomId);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        booking.TotalPrice.Amount.Should().Be(350.50m);
        result.Should().Be(true);
    }

    [Fact]
    public async Task Handle_CallsBookingsRepository()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId, 350.50m);
        var booking = CreateBooking(hotelId, roomId);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);

        // Act
        await _useCase.Handle(command);

        // Assert
        await _bookingsRepositoryMock.Received(1).GetById(bookingId);
        await _bookingsRepositoryMock.Received(1).Update(Arg.Any<Booking>());
    }

    [Fact]
    public async Task Handle_CallsUnitOfWorkPersist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId, 350.50m);
        var booking = CreateBooking(hotelId, roomId);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);

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
        var command = CreateValidCommand(bookingId, 350.50m);

        _bookingsRepositoryMock.GetById(bookingId).Returns((Booking?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();

        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_ThrowsBusinessRuleException_WhenBookingIsCompleted()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId, 350.50m);
        var booking = CreateBooking(hotelId, roomId, BookingStatus.Complete);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Booking is already completed, unable to modify total price");
    }

    [Fact]
    public async Task Handle_ThrowsBusinessRuleException_WhenBookingIsCancelled()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId, 350.50m);
        var booking = CreateBooking(hotelId, roomId, BookingStatus.Cancelled);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Booking is already cancelled, unable to modify total price");
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenUpdateFails()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId, 350.50m);
        var booking = CreateBooking(hotelId, roomId);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
        _bookingsRepositoryMock.Update(Arg.Any<Booking>()).Throws<InvalidOperationException>();

        // Act
        await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.Handle(command));

        // Assert
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    private static UpdateTotalPriceCommand CreateValidCommand(Guid bookingId, decimal totalPrice)
    {
        return new UpdateTotalPriceCommand
        {
            BookingId = bookingId,
            TotalPrice = totalPrice
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
}