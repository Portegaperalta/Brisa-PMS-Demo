using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Bookings.Commands.UpdateGuestCount;
using BrisaPMS.Domain.Booking;
using BrisaPMS.Domain.Bookings;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Commands.UpdateGuestCount;

public class UpdateGuestCountUseCaseTests
{
    private readonly IBookingsRepository _bookingsRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UpdateGuestCountUseCase _useCase;

    public UpdateGuestCountUseCaseTests()
    {
        _bookingsRepositoryMock = Substitute.For<IBookingsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new UpdateGuestCountUseCase(_bookingsRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_UpdatesGuestCountAndReturnsTrue()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId, 3, 2);
        var booking = CreateBooking(hotelId, roomId);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        booking.GuestCount.NumberOfAdults.Should().Be(3);
        booking.GuestCount.NumberOfChildren.Should().Be(2);
        result.Should().Be(true);
    }

    [Fact]
    public async Task Handle_CallsBookingsRepository()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId, 3, 2);
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
        var command = CreateValidCommand(bookingId, 3, 2);
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
        var command = CreateValidCommand(bookingId, 3, 2);

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
        var command = CreateValidCommand(bookingId, 3, 2);
        var booking = CreateBooking(hotelId, roomId, BookingStatus.Complete);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Booking is already completed, unable to modify guest count");
    }

    [Fact]
    public async Task Handle_ThrowsBusinessRuleException_WhenBookingIsCancelled()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId, 3, 2);
        var booking = CreateBooking(hotelId, roomId, BookingStatus.Cancelled);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("Booking is already cancelled, unable to modify guest count");
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenUpdateFails()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var command = CreateValidCommand(bookingId, 3, 2);
        var booking = CreateBooking(hotelId, roomId);

        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
        _bookingsRepositoryMock.Update(Arg.Any<Booking>()).Throws<InvalidOperationException>();

        // Act
        await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.Handle(command));

        // Assert
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    private static UpdateGuestCountCommand CreateValidCommand(Guid bookingId, int numberOfAdults, int numberOfChildren)
    {
        return new UpdateGuestCountCommand
        {
            BookingId = bookingId,
            NumberOfAdults = numberOfAdults,
            NumberOfChildren = numberOfChildren
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