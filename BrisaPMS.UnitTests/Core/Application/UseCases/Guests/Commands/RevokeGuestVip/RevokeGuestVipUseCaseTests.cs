using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Guests.Commands.RevokeGuestVip;
using BrisaPMS.Domain.Guest;
using BrisaPMS.Domain.Guests;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Guests.Commands.RevokeGuestVip;

public class RevokeGuestVipUseCaseTests
{
    private readonly IGuestsRepository _guestsRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly RevokeGuestVipUseCase _useCase;

    public RevokeGuestVipUseCaseTests()
    {
        _guestsRepositoryMock = Substitute.For<IGuestsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new RevokeGuestVipUseCase(_guestsRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_RevokesGuestVip()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var guest = CreateGuest(guestId, isVip: true);
        var command = new RevokeGuestVipCommand { GuestId = guestId };

        _guestsRepositoryMock.GetById(guestId).Returns(guest);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        guest.IsVip.Should().BeFalse();
        await _guestsRepositoryMock.Received(1).Update(guest);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenGuestDoesNotExist()
    {
        // Arrange
        var command = new RevokeGuestVipCommand { GuestId = Guid.NewGuid() };

        _guestsRepositoryMock.GetById(command.GuestId).Returns((Guest?)null);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _guestsRepositoryMock.DidNotReceive().Update(Arg.Any<Guest>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenRepositoryUpdateFails()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var guest = CreateGuest(guestId, isVip: true);
        var command = new RevokeGuestVipCommand { GuestId = guestId };

        _guestsRepositoryMock.GetById(guestId).Returns(guest);
        _guestsRepositoryMock.Update(Arg.Any<Guest>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    private static Guest CreateGuest(Guid? guestId = null, bool isVip = true)
    {
        var guest = new Guest.Builder(
            Guid.NewGuid(),
            "John",
            "Doe",
            GuestDocumentType.Passport,
            "A1234567",
            new Email("guest@example.com"),
            new PhoneNumber("+18095551234"),
            CurrencyCode.USD,
            isVip)
            .WithCountry("Dominican Republic")
            .WithRnc(new Rnc("123456789"))
            .WithPreferredLanguage("English")
            .WithNotes("Frequent guest")
            .Build();

        if (guestId.HasValue)
            typeof(Guest).GetProperty("Id")!.SetValue(guest, guestId.Value);

        return guest;
    }
}
