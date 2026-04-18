using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Guests.Queries.GetGuestByHotelId;
using BrisaPMS.Domain.Guest;
using BrisaPMS.Domain.Guests;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Guests.Queries.GetGuestByHotelId;

public class GetGuestByHotelIdUseCaseTests
{
    private readonly IGuestsRepository _guestsRepositoryMock;
    private readonly IHotelsRepository _hotelsRepositoryMock;
    private readonly GetGuestByHotelIdUseCase _useCase;

    public GetGuestByHotelIdUseCaseTests()
    {
        _guestsRepositoryMock = Substitute.For<IGuestsRepository>();
        _hotelsRepositoryMock = Substitute.For<IHotelsRepository>();
        _useCase = new GetGuestByHotelIdUseCase(_guestsRepositoryMock, _hotelsRepositoryMock);
    }

    [Fact]
    public async Task Handle_ReturnsGuestDto()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var guest = CreateGuest(hotelId: hotelId);
        var query = new GetGuestByHotelIdQuery { HotelId = hotelId };

        _hotelsRepositoryMock.Exists(hotelId).Returns(true);
        _guestsRepositoryMock.GetByHotelIdAsync(hotelId).Returns(guest);

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(guest.Id);
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
        await _hotelsRepositoryMock.Received(1).Exists(hotelId);
        await _guestsRepositoryMock.Received(1).GetByHotelIdAsync(hotelId);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenHotelHasNoGuest()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var query = new GetGuestByHotelIdQuery { HotelId = hotelId };

        _hotelsRepositoryMock.Exists(hotelId).Returns(true);
        _guestsRepositoryMock.GetByHotelIdAsync(hotelId).ReturnsNull();

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().BeNull();
        await _hotelsRepositoryMock.Received(1).Exists(hotelId);
        await _guestsRepositoryMock.Received(1).GetByHotelIdAsync(hotelId);
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var query = new GetGuestByHotelIdQuery { HotelId = hotelId };

        _hotelsRepositoryMock.Exists(hotelId).Returns(false);

        // Act
        var act = async () => await _useCase.Handle(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _guestsRepositoryMock.DidNotReceive().GetByHotelIdAsync(Arg.Any<Guid>());
    }

    private static Guest CreateGuest(Guid? guestId = null, Guid? hotelId = null)
    {
        var guest = new Guest.Builder(
            hotelId ?? Guid.NewGuid(),
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
