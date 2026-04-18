using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Guests.Queries.GetAllGuestsByHotelId;
using BrisaPMS.Domain.Guest;
using BrisaPMS.Domain.Guests;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;
using NSubstitute;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Guests.Queries.GetAllGuestsByHotelId;

public class GetAllGuestsByHotelIdUseCaseTests
{
    private readonly IGuestsRepository _guestsRepositoryMock;
    private readonly IHotelsRepository _hotelsRepositoryMock;
    private readonly GetAllGuestsByHotelIdUseCase _useCase;

    public GetAllGuestsByHotelIdUseCaseTests()
    {
        _guestsRepositoryMock = Substitute.For<IGuestsRepository>();
        _hotelsRepositoryMock = Substitute.For<IHotelsRepository>();
        _useCase = new GetAllGuestsByHotelIdUseCase(_guestsRepositoryMock, _hotelsRepositoryMock);
    }

    [Fact]
    public async Task Handle_ReturnsListOfGuestDtos()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var guests = new List<Guest>
        {
            CreateGuest(Guid.NewGuid(), hotelId, "John", "Doe"),
            CreateGuest(Guid.NewGuid(), hotelId, "Jane", "Smith")
        };
        var query = new GetAllGuestsByHotelIdQuery { HotelId = hotelId };

        _hotelsRepositoryMock.Exists(hotelId).Returns(true);
        _guestsRepositoryMock.GetAllByHotelIdAsync(hotelId).Returns(guests);

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(
            guests.Select(guest => new
            {
                guest.Id,
                guest.HotelId,
                guest.FirstName,
                guest.LastName,
                DocumentType = guest.DocumentType.ToString(),
                guest.DocumentNumber,
                guest.Country,
                Rnc = guest.Rnc!.Value,
                Email = guest.Email.Value,
                PhoneNumber = guest.PhoneNumber.Value,
                PreferredCurrency = guest.PreferredCurrency.ToString(),
                guest.PreferredLanguage,
                guest.IsVip,
                IsBlackListed = guest.IsBlackListed,
                guest.BlackListedReason,
                guest.Notes
            }));
        await _hotelsRepositoryMock.Received(1).Exists(hotelId);
        await _guestsRepositoryMock.Received(1).GetAllByHotelIdAsync(hotelId);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenHotelHasNoGuests()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var query = new GetAllGuestsByHotelIdQuery { HotelId = hotelId };

        _hotelsRepositoryMock.Exists(hotelId).Returns(true);
        _guestsRepositoryMock.GetAllByHotelIdAsync(hotelId).Returns([]);

        // Act
        var result = await _useCase.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        await _hotelsRepositoryMock.Received(1).Exists(hotelId);
        await _guestsRepositoryMock.Received(1).GetAllByHotelIdAsync(hotelId);
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenHotelDoesNotExist()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var query = new GetAllGuestsByHotelIdQuery { HotelId = hotelId };

        _hotelsRepositoryMock.Exists(hotelId).Returns(false);

        // Act
        var act = async () => await _useCase.Handle(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _guestsRepositoryMock.DidNotReceive().GetAllByHotelIdAsync(Arg.Any<Guid>());
    }

    private static Guest CreateGuest(Guid guestId, Guid hotelId, string firstName, string lastName)
    {
        var guest = new Guest.Builder(
            hotelId,
            firstName,
            lastName,
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

        typeof(Guest).GetProperty("Id")!.SetValue(guest, guestId);

        return guest;
    }
}
