using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Guests.Queries.GetGuestById;
using BrisaPMS.Domain.Guest;
using BrisaPMS.Domain.Guests;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Guests.Queries.GetGuestById;

public class GetGuestByIdUseCaseTests
{
    private readonly IGuestsRepository _repositoryMock;
    private readonly GetGuestByIdUseCase _useCase;

    public GetGuestByIdUseCaseTests()
    {
        _repositoryMock = Substitute.For<IGuestsRepository>();
        _useCase = new GetGuestByIdUseCase(_repositoryMock);
    }

    [Fact]
    public async Task Handle_ReturnsGuestDto()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var guest = CreateGuest(guestId);
        var query = new GetGuestByIdQuery { GuestId = guestId };

        _repositoryMock.GetById(guestId).Returns(guest);

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(guest.Id);
        result.HotelId.Should().Be(guest.HotelId);
        result.FirstName.Should().Be(guest.FirstName);
        result.LastName.Should().Be(guest.LastName);
        result.DocumentType.Should().Be(guest.DocumentType.ToString());
        result.DocumentNumber.Should().Be(guest.DocumentNumber);
        result.Country.Should().Be(guest.Country);
        result.Rnc.Should().Be(guest.Rnc!.Value);
        result.Email.Should().Be(guest.Email.Value);
        result.PhoneNumber.Should().Be(guest.PhoneNumber.Value);
        result.PreferredCurrency.Should().Be(guest.PreferredCurrency.ToString());
        result.PreferredLanguage.Should().Be(guest.PreferredLanguage);
        result.IsVip.Should().Be(guest.IsVip);
        result.IsBlackListed.Should().Be(guest.IsBlackListed);
        result.BlackListedReason.Should().Be(guest.BlackListedReason);
        result.Notes.Should().Be(guest.Notes);
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenGuestDoesNotExist()
    {
        // Arrange
        var guestId = Guid.NewGuid();
        var query = new GetGuestByIdQuery { GuestId = guestId };

        _repositoryMock.GetById(guestId).ReturnsNull();

        // Act
        var act = async () => await _useCase.Handle(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
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
