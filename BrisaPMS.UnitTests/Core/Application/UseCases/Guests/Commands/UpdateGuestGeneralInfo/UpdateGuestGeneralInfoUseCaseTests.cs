using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestGeneralInfo;
using BrisaPMS.Domain.Guest;
using BrisaPMS.Domain.Guests;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Guests.Commands.UpdateGuestGeneralInfo;

public class UpdateGuestGeneralInfoUseCaseTests
{
    private readonly IGuestsRepository _guestsRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UpdateGuestGeneralInfoUseCase _useCase;

    public UpdateGuestGeneralInfoUseCaseTests()
    {
        _guestsRepositoryMock = Substitute.For<IGuestsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new UpdateGuestGeneralInfoUseCase(_guestsRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_UpdatesGuestGeneralInfo()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var guest = CreateGuest(guestId);
        var command = CreateCommand(guestId, "Jane", "Smith", "Germany", "German", "Needs late check-in");

        _guestsRepositoryMock.GetById(guestId).Returns(guest);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        guest.FirstName.Should().Be(command.FirstName);
        guest.LastName.Should().Be(command.LastName);
        guest.Country.Should().Be(command.Country);
        guest.PreferredLanguage.Should().Be(command.PreferredLanguage);
        guest.Notes.Should().Be(command.Notes);
        await _guestsRepositoryMock.Received(1).Update(guest);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_UpdatesOnlyRequiredFields_WhenOptionalFieldsAreNotProvided()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var guest = CreateGuest(guestId);
        var originalCountry = guest.Country;
        var originalPreferredLanguage = guest.PreferredLanguage;
        var originalNotes = guest.Notes;
        var command = CreateCommand(guestId, "Jane", "Smith", null, null, null);

        _guestsRepositoryMock.GetById(guestId).Returns(guest);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        guest.FirstName.Should().Be(command.FirstName);
        guest.LastName.Should().Be(command.LastName);
        guest.Country.Should().Be(originalCountry);
        guest.PreferredLanguage.Should().Be(originalPreferredLanguage);
        guest.Notes.Should().Be(originalNotes);
        await _guestsRepositoryMock.Received(1).Update(guest);
        await _unitOfWorkMock.Received(1).Persist();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenGuestDoesNotExist()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), "Jane", "Smith", "Germany", "German", "Needs late check-in");

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
        var guest = CreateGuest(guestId);
        var command = CreateCommand(guestId, "Jane", "Smith", "Germany", "German", "Needs late check-in");

        _guestsRepositoryMock.GetById(guestId).Returns(guest);
        _guestsRepositoryMock.Update(Arg.Any<Guest>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    private static UpdateGuestGeneralInfoCommand CreateCommand(
        Guid guestId,
        string firstName,
        string lastName,
        string? country,
        string? preferredLanguage,
        string? notes)
    {
        return new UpdateGuestGeneralInfoCommand
        {
            GuestId = guestId,
            FirstName = firstName,
            LastName = lastName,
            Country = country,
            PreferredLanguage = preferredLanguage,
            Notes = notes
        };
    }

    private static Guest CreateGuest(Guid? guestId = null)
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
            true)
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
