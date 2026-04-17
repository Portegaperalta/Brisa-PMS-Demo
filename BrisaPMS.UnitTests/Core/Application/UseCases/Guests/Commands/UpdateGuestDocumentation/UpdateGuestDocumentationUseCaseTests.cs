using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestDocumentation;
using BrisaPMS.Domain.Guest;
using BrisaPMS.Domain.Guests;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Guests.Commands.UpdateGuestDocumentation;

public class UpdateGuestDocumentationUseCaseTests
{
    private readonly IGuestsRepository _guestsRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly UpdateGuestDocumentationUseCase _useCase;

    public UpdateGuestDocumentationUseCaseTests()
    {
        _guestsRepositoryMock = Substitute.For<IGuestsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new UpdateGuestDocumentationUseCase(_guestsRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_UpdatesGuestDocumentation()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var guest = CreateGuest(guestId);
        var command = CreateCommand(guestId, "IdCard", "00112345678");

        _guestsRepositoryMock.GetById(guestId).Returns(guest);

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        guest.DocumentType.Should().Be(GuestDocumentType.IdCard);
        guest.DocumentNumber.Should().Be(command.DocumentNumber);
        await _guestsRepositoryMock.Received(1).Update(guest);
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenGuestDoesNotExist()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), "IdCard", "00112345678");

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
        var command = CreateCommand(guestId, "IdCard", "00112345678");

        _guestsRepositoryMock.GetById(guestId).Returns(guest);
        _guestsRepositoryMock.Update(Arg.Any<Guest>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    private static UpdateGuestDocumentationCommand CreateCommand(Guid guestId, string documentType, string documentNumber)
    {
        return new UpdateGuestDocumentationCommand
        {
            GuestId = guestId,
            DocumentType = documentType,
            DocumentNumber = documentNumber
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
