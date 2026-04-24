using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Stays.Commands.CreateStay;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Stays;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Stays.Commands.CreateStay;

public class CreateStayUseCaseTests
{
    private readonly IStaysRepository _staysRepositoryMock;
    private readonly IGuestsRepository _guestsRepositoryMock;
    private readonly IBookingsRepository _bookingsRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CreateStayUseCase _useCase;

    public CreateStayUseCaseTests()
    {
        _staysRepositoryMock = Substitute.For<IStaysRepository>();
        _guestsRepositoryMock = Substitute.For<IGuestsRepository>();
        _bookingsRepositoryMock = Substitute.For<IBookingsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _useCase = new CreateStayUseCase(
            _staysRepositoryMock,
            _guestsRepositoryMock,
            _bookingsRepositoryMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_CreatesStayAndReturnsStayId()
    {
        // Arrange
        var command = new CreateStayCommand
        {
            GuestId = Guid.NewGuid(),
            BookingId = Guid.NewGuid()
        };

        _guestsRepositoryMock.Exists(command.GuestId).Returns(true);
        _bookingsRepositoryMock.Exists(command.BookingId).Returns(true);
        _bookingsRepositoryMock.GetBookingStatusAsync(command.BookingId).Returns("Pending");

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        result.Should().NotBe(Guid.Empty);
        await _guestsRepositoryMock.Received(1).Exists(command.GuestId);
        await _bookingsRepositoryMock.Received(1).Exists(command.BookingId);
        await _bookingsRepositoryMock.Received(1).GetBookingStatusAsync(command.BookingId);
        await _staysRepositoryMock.Received(1).Create(Arg.Is<Stay>(stay =>
            stay.GuestId == command.GuestId &&
            stay.BookingId == command.BookingId &&
            stay.NightCount == 0 &&
            stay.Status == StayStatus.InProgress));

        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenGuestDoesNotExist()
    {
        // Arrange
        var command = new CreateStayCommand
        {
            GuestId = Guid.NewGuid(),
            BookingId = Guid.NewGuid()
        };

        _guestsRepositoryMock.Exists(command.GuestId).Returns(false);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _bookingsRepositoryMock.DidNotReceive().Exists(Arg.Any<Guid>());
        await _staysRepositoryMock.DidNotReceive().Create(Arg.Any<Stay>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenBookingDoesNotExist()
    {
        // Arrange
        var command = new CreateStayCommand
        {
            GuestId = Guid.NewGuid(),
            BookingId = Guid.NewGuid()
        };

        _guestsRepositoryMock.Exists(command.GuestId).Returns(true);
        _bookingsRepositoryMock.Exists(command.BookingId).Returns(false);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _bookingsRepositoryMock.DidNotReceive().GetBookingStatusAsync(Arg.Any<Guid>());
        await _staysRepositoryMock.DidNotReceive().Create(Arg.Any<Stay>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Theory]
    [InlineData("Complete")]
    [InlineData("Cancelled")]
    public async Task Handle_ThrowsBusinessRuleException_WhenBookingCannotCreateStay(string bookingStatus)
    {
        // Arrange
        var command = new CreateStayCommand
        {
            GuestId = Guid.NewGuid(),
            BookingId = Guid.NewGuid()
        };

        _guestsRepositoryMock.Exists(command.GuestId).Returns(true);
        _bookingsRepositoryMock.Exists(command.BookingId).Returns(true);
        _bookingsRepositoryMock.GetBookingStatusAsync(command.BookingId).Returns(bookingStatus);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<BusinessRuleException>();
        await _staysRepositoryMock.DidNotReceive().Create(Arg.Any<Stay>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenRepositoryCreateFails()
    {
        // Arrange
        var command = new CreateStayCommand
        {
            GuestId = Guid.NewGuid(),
            BookingId = Guid.NewGuid()
        };

        _guestsRepositoryMock.Exists(command.GuestId).Returns(true);
        _bookingsRepositoryMock.Exists(command.BookingId).Returns(true);
        _bookingsRepositoryMock.GetBookingStatusAsync(command.BookingId).Returns("Pending");
        _staysRepositoryMock.Create(Arg.Any<Stay>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }
}
