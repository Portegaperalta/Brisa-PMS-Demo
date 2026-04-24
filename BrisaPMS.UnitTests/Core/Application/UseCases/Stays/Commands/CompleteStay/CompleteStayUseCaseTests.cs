using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Stays.Commands.CompleteStay;
using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.Stays;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using BrisaPMS.Domain.Booking;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Stays.Commands.CompleteStay;

public class CompleteStayUseCaseTests
{
    private readonly IStaysRepository _staysRepositoryMock;
    private readonly IBookingsRepository _bookingsRepositoryMock;
    private readonly IRoomsRepository _roomsRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CompleteStayUseCase _useCase;

    public CompleteStayUseCaseTests()
    {
        _staysRepositoryMock = Substitute.For<IStaysRepository>();
        _bookingsRepositoryMock = Substitute.For<IBookingsRepository>();
        _roomsRepositoryMock = Substitute.For<IRoomsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new CompleteStayUseCase(
            _staysRepositoryMock,
            _bookingsRepositoryMock,
            _roomsRepositoryMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_CompletesStayBookingAndMarksRoomAsDirty()
    {
        // Arrange
        var stayId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var command = new CompleteStayCommand { StayId = stayId };
        var stay = StayTestData.CreateStay(stayId: stayId, bookingId: bookingId);
        var booking = StayTestData.CreateBooking(bookingId: bookingId, roomId: roomId);
        var room = StayTestData.CreateRoom(roomId: roomId, hygieneStatus: RoomHygieneStatus.Clean);

        _staysRepositoryMock.GetById(stayId).Returns(stay);
        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
        _roomsRepositoryMock.GetById(roomId).Returns(room);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        result.Should().BeTrue();
        stay.Status.Should().Be(StayStatus.Complete);
        stay.TimeInterval.ActualCheckOut.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        booking.Status.Should().Be(BookingStatus.Complete);
        room.HygieneStatus.Should().Be(RoomHygieneStatus.Dirty);
        await _staysRepositoryMock.Received(1).Update(stay);
        await _bookingsRepositoryMock.Received(1).Update(booking);
        await _roomsRepositoryMock.Received(1).Update(room);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenStayDoesNotExist()
    {
        // Arrange
        var command = new CompleteStayCommand { StayId = Guid.NewGuid() };

        _staysRepositoryMock.GetById(command.StayId).Returns((Stay?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _bookingsRepositoryMock.DidNotReceive().GetById(Arg.Any<Guid>());
        await _roomsRepositoryMock.DidNotReceive().GetById(Arg.Any<Guid>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenRepositoryUpdateFails()
    {
        // Arrange
        var stayId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var command = new CompleteStayCommand { StayId = stayId };
        var stay = StayTestData.CreateStay(stayId: stayId, bookingId: bookingId);
        var booking = StayTestData.CreateBooking(bookingId: bookingId, roomId: roomId);
        var room = StayTestData.CreateRoom(roomId: roomId);

        _staysRepositoryMock.GetById(stayId).Returns(stay);
        _bookingsRepositoryMock.GetById(bookingId).Returns(booking);
        _roomsRepositoryMock.GetById(roomId).Returns(room);
        _staysRepositoryMock.Update(Arg.Any<Stay>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }
}
