using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Guests.Commands.CreateGuest;
using BrisaPMS.Application.UseCases.Guests.Shared;
using BrisaPMS.Domain.Guest;
using BrisaPMS.Domain.Guests;
using BrisaPMS.Domain.Shared.Enums;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Guests.Commands.CreateGuest;

public class CreateGuestUseCaseTests
{
    private readonly IGuestsRepository _guestsRepositoryMock;
    private readonly IHotelsRepository _hotelsRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CreateGuestUseCase _useCase;

    public CreateGuestUseCaseTests()
    {
        _guestsRepositoryMock = Substitute.For<IGuestsRepository>();
        _hotelsRepositoryMock = Substitute.For<IHotelsRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _useCase = new CreateGuestUseCase(_guestsRepositoryMock, _hotelsRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_CreatesGuestAndReturnsGuestDto()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var command = CreateValidCommand(hotelId);

        _hotelsRepositoryMock.Exists(hotelId).Returns(true);
        _guestsRepositoryMock.Create(Arg.Any<Guest>())
            .Returns(callInfo => callInfo.Arg<Guest>());

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        await _hotelsRepositoryMock.Received(1).Exists(hotelId);
        await _guestsRepositoryMock.Received(1).Create(Arg.Is<Guest>(guest =>
            guest.HotelId == command.HotelId &&
            guest.FirstName == command.FirstName &&
            guest.LastName == command.LastName &&
            guest.DocumentType == Enum.Parse<GuestDocumentType>(command.DocumentType) &&
            guest.DocumentNumber == command.DocumentNumber &&
            guest.Country == command.Country &&
            guest.Rnc!.Value == command.Rnc &&
            guest.Email.Value == command.Email &&
            guest.PhoneNumber.Value == command.PhoneNumber &&
            guest.PreferredCurrency == Enum.Parse<CurrencyCode>(command.PreferredCurrency) &&
            guest.PreferredLanguage == command.PreferredLanguage &&
            guest.IsVip == command.IsVip &&
            guest.Notes == command.Notes));
        await _unitOfWorkMock.Received(1).Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
        result.Should().NotBeNull();
        result.Should().BeOfType<GuestDto>();
        result.Id.Should().NotBe(Guid.Empty);
        result.Should().BeEquivalentTo(new
        {
            command.HotelId,
            command.FirstName,
            command.LastName,
            command.DocumentType,
            command.DocumentNumber,
            command.Country,
            command.Rnc,
            command.Email,
            command.PhoneNumber,
            command.PreferredCurrency,
            command.PreferredLanguage,
            command.IsVip,
            IsBlackListed = false,
            BlackListedReason = (string?)null,
            command.Notes
        }, options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task Handle_CreatesGuestAndReturnsGuestDto_WhenOptionalFieldsAreNotProvided()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var command = CreateCommand
        (
            hotelId,
            "John",
            "Doe",
            "Passport",
            "A1234567",
            null,
            null,
            "guest@example.com",
            "+18095551234",
            "USD",
            null,
            true,
            null
        );

        _hotelsRepositoryMock.Exists(hotelId).Returns(true);
        _guestsRepositoryMock.Create(Arg.Any<Guest>())
            .Returns(callInfo => callInfo.Arg<Guest>());

        // Act
        var result = await _useCase.Handle(command);

        // Assert
        await _guestsRepositoryMock.Received(1).Create(Arg.Is<Guest>(guest =>
            guest.Country == null &&
            guest.Rnc == null &&
            guest.PreferredLanguage == null &&
            guest.Notes == null));

        await _unitOfWorkMock.Received(1).Persist();
        result.Should().NotBeNull();
        result.Country.Should().BeNull();
        result.Rnc.Should().BeNull();
        result.PreferredLanguage.Should().BeNull();
        result.Notes.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ThrowsNotFoundException_WhenHotelDoesNotExist()
    {
        // Arrange
        var command = CreateValidCommand(Guid.NewGuid());

        _hotelsRepositoryMock.Exists(command.HotelId).Returns(false);

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        await _guestsRepositoryMock.DidNotReceive().Create(Arg.Any<Guest>());
        await _unitOfWorkMock.DidNotReceive().Persist();
        await _unitOfWorkMock.DidNotReceive().Revert();
    }

    [Fact]
    public async Task Handle_RevertsUnitOfWork_WhenRepositoryCreateFails()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var command = CreateValidCommand(hotelId);

        _hotelsRepositoryMock.Exists(hotelId).Returns(true);
        _guestsRepositoryMock.Create(Arg.Any<Guest>()).Throws<InvalidOperationException>();

        // Act
        var act = async () => await _useCase.Handle(command);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        await _unitOfWorkMock.Received(1).Revert();
        await _unitOfWorkMock.DidNotReceive().Persist();
    }

    private static CreateGuestCommand CreateValidCommand(Guid hotelId)
    {
        return CreateCommand
        (
            hotelId,
            "John",
            "Doe",
            "Passport",
            "A1234567",
            "Dominican Republic",
            "123456789",
            "guest@example.com",
            "+18095551234",
            "USD",
            "English",
            true,
            "Frequent guest"
        );
    }

    private static CreateGuestCommand CreateCommand(
        Guid hotelId,
        string firstName,
        string lastName,
        string documentType,
        string documentNumber,
        string? country,
        string? rnc,
        string email,
        string phoneNumber,
        string preferredCurrency,
        string? preferredLanguage,
        bool isVip,
        string? notes)
    {
        return new CreateGuestCommand
        {
            HotelId = hotelId,
            FirstName = firstName,
            LastName = lastName,
            DocumentType = documentType,
            DocumentNumber = documentNumber,
            Country = country,
            Rnc = rnc,
            Email = email,
            PhoneNumber = phoneNumber,
            PreferredCurrency = preferredCurrency,
            PreferredLanguage = preferredLanguage,
            IsVip = isVip,
            Notes = notes
        };
    }
}
