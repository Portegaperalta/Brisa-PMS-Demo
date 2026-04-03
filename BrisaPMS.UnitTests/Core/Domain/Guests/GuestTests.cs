using BrisaPMS.Domain.Guest;
using BrisaPMS.Domain.Guests;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Guests;

public class GuestTests
{
    [Fact]
    public void Constructor_ShouldCreateGuest_WhenValuesAreValid()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var rnc = CreateRnc();
        var email = CreateEmail();
        var phoneNumber = CreatePhoneNumber();

        // Act
        var result = new Guest(
            hotelId,
            "John",
            "Doe",
            GuestDocumentType.Passport,
            "A1234567",
            phoneNumber,
            CurrencyCode.USD,
            true,
            "United States",
            rnc,
            email,
            "English",
            "VIP guest");

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
        result.HotelId.Should().Be(hotelId);
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.DocumentType.Should().Be(GuestDocumentType.Passport);
        result.DocumentNumber.Should().Be("A1234567");
        result.Country.Should().Be("United States");
        result.Rnc.Should().Be(rnc);
        result.Email.Should().Be(email);
        result.PhoneNumber.Should().Be(phoneNumber);
        result.PreferredCurrency.Should().Be(CurrencyCode.USD);
        result.PreferredLanguage.Should().Be("English");
        result.IsVip.Should().BeTrue();
        result.IsBlackListed.Should().BeFalse();
        result.BlackListedReason.Should().BeNull();
        result.Notes.Should().Be("VIP guest");
    }

    [Fact]
    public void Constructor_ShouldCreateGuest_WhenOptionalValuesAreNotProvided()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var phoneNumber = CreatePhoneNumber();

        // Act
        var result = new Guest(
            hotelId,
            "John",
            "Doe",
            GuestDocumentType.IdCard,
            "00112345678",
            phoneNumber,
            CurrencyCode.DOP,
            false);

        // Assert
        result.Country.Should().BeNull();
        result.Rnc.Should().BeNull();
        result.Email.Should().BeNull();
        result.PhoneNumber.Should().Be(phoneNumber);
        result.PreferredLanguage.Should().BeNull();
        result.Notes.Should().BeNull();
        result.IsVip.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenFirstNameIsNullOrWhiteSpace(string? firstName)
    {
        // Arrange
        const string lastName = "Doe";

        // Act
        Action act = () => _ = new Guest(Guid.NewGuid(), firstName!, lastName, GuestDocumentType.IdCard, "00112345678", CreatePhoneNumber(), CurrencyCode.DOP, false);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenLastNameIsNullOrWhiteSpace(string? lastName)
    {
        // Arrange
        const string firstName = "John";

        // Act
        Action act = () => _ = new Guest(Guid.NewGuid(), firstName, lastName!, GuestDocumentType.IdCard, "00112345678", CreatePhoneNumber(), CurrencyCode.DOP, false);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenDocumentTypeIsInvalid()
    {
        // Arrange
        var invalidDocumentType = (GuestDocumentType)999;

        // Act
        Action act = () => _ = new Guest(Guid.NewGuid(), "John", "Doe", invalidDocumentType, "00112345678", CreatePhoneNumber(), CurrencyCode.DOP, false);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenDocumentNumberIsEmpty()
    {
        // Arrange
        const string documentNumber = " ";

        // Act
        Action act = () => _ = new Guest(Guid.NewGuid(), "John", "Doe", GuestDocumentType.IdCard, documentNumber, CreatePhoneNumber(), CurrencyCode.DOP, false);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenPreferredCurrencyIsInvalid()
    {
        // Arrange
        var invalidCurrency = (CurrencyCode)999;

        // Act
        Action act = () => _ = new Guest(Guid.NewGuid(), "John", "Doe", GuestDocumentType.IdCard, "00112345678", CreatePhoneNumber(), invalidCurrency, false);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void ChangeFirstName_ShouldUpdateFirstName_WhenValueIsValid()
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        guest.ChangeFirstName("Jane");

        // Assert
        guest.FirstName.Should().Be("Jane");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    public void ChangeFirstName_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrWhiteSpace(string? newFirstName)
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        Action act = () => guest.ChangeFirstName(newFirstName!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void ChangeLastName_ShouldUpdateLastName_WhenValueIsValid()
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        guest.ChangeLastName("Smith");

        // Assert
        guest.LastName.Should().Be("Smith");
    }

    [Fact]
    public void ChangeDocumentType_ShouldUpdateDocumentType_WhenValueIsValid()
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        guest.ChangeDocumentType(GuestDocumentType.Passport);

        // Assert
        guest.DocumentType.Should().Be(GuestDocumentType.Passport);
    }

    [Fact]
    public void ChangeDocumentType_ShouldThrowBusinessRuleException_WhenValueIsInvalid()
    {
        // Arrange
        var guest = CreateGuest();
        var invalidDocumentType = (GuestDocumentType)999;

        // Act
        Action act = () => guest.ChangeDocumentType(invalidDocumentType);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void ChangeDocumentNumber_ShouldUpdateDocumentNumber_WhenValueIsValid()
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        guest.ChangeDocumentNumber("B7654321");

        // Assert
        guest.DocumentNumber.Should().Be("B7654321");
    }

    [Fact]
    public void ChangeDocumentNumber_ShouldThrowEmptyRequiredFieldException_WhenValueIsWhiteSpace()
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        Action act = () => guest.ChangeDocumentNumber(" ");

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void ChangeCountry_ShouldUpdateCountry_WhenValueIsValid()
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        guest.ChangeCountry("Germany");

        // Assert
        guest.Country.Should().Be("Germany");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    public void ChangeCountry_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrWhiteSpace(string? newCountry)
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        Action act = () => guest.ChangeCountry(newCountry!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void ChangeRnc_ShouldUpdateRnc_WhenValueIsValid()
    {
        // Arrange
        var guest = CreateGuest();
        var newRnc = new Rnc("00112345678");

        // Act
        guest.ChangeRnc(newRnc);

        // Assert
        guest.Rnc.Should().Be(newRnc);
    }

    [Fact]
    public void ChangeEmail_ShouldUpdateEmail_WhenValueIsValid()
    {
        // Arrange
        var guest = CreateGuest();
        var newEmail = new Email("guest.updated@example.com");

        // Act
        guest.ChangeEmail(newEmail);

        // Assert
        guest.Email.Should().Be(newEmail);
    }

    [Fact]
    public void ChangePhoneNumber_ShouldUpdatePhoneNumber_WhenValueIsValid()
    {
        // Arrange
        var guest = CreateGuest();
        var newPhoneNumber = new PhoneNumber("+1 829 555 4321");

        // Act
        guest.ChangePhoneNumber(newPhoneNumber);

        // Assert
        guest.PhoneNumber.Should().Be(newPhoneNumber);
    }

    [Fact]
    public void ChangePreferredCurrency_ShouldUpdatePreferredCurrency_WhenValueIsValid()
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        guest.ChangePreferredCurrency(CurrencyCode.EUR);

        // Assert
        guest.PreferredCurrency.Should().Be(CurrencyCode.EUR);
    }

    [Fact]
    public void ChangePreferredLanguage_ShouldUpdatePreferredLanguage_WhenValueIsValid()
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        guest.ChangePreferredLanguage("Spanish");

        // Assert
        guest.PreferredLanguage.Should().Be("Spanish");
    }

    [Fact]
    public void ChangePreferredCurrency_ShouldThrowBusinessRuleException_WhenValueIsInvalid()
    {
        // Arrange
        var guest = CreateGuest();
        var invalidCurrency = (CurrencyCode)999;

        // Act
        Action act = () => guest.ChangePreferredCurrency(invalidCurrency);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    public void ChangePreferredLanguage_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrWhiteSpace(string? newPreferredLanguage)
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        Action act = () => guest.ChangePreferredLanguage(newPreferredLanguage!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void DisableVip_ShouldSetIsVipToFalse()
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        guest.DisableVip();

        // Assert
        guest.IsVip.Should().BeFalse();
    }

    [Fact]
    public void EnableVip_ShouldSetIsVipToTrue()
    {
        // Arrange
        var guest = CreateGuest();
        guest.DisableVip();

        // Act
        guest.EnableVip();

        // Assert
        guest.IsVip.Should().BeTrue();
    }

    [Fact]
    public void BlackList_ShouldSetBlackListStateAndReason_WhenReasonIsValid()
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        guest.BlackList("Payment fraud");

        // Assert
        guest.IsBlackListed.Should().BeTrue();
        guest.BlackListedReason.Should().Be("Payment fraud");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    public void BlackList_ShouldThrowBusinessRuleException_WhenReasonIsNullOrWhiteSpace(string? blackListedReason)
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        Action act = () => guest.BlackList(blackListedReason!);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void DisableBlackList_ShouldSetIsBlackListedToFalse()
    {
        // Arrange
        var guest = CreateGuest();
        guest.BlackList("Initial reason");

        // Act
        guest.DisableBlackList();

        // Assert
        guest.IsBlackListed.Should().BeFalse();
    }

    [Fact]
    public void ChangeBlackListedReason_ShouldUpdateBlackListedReason()
    {
        // Arrange
        var guest = CreateGuest();
        guest.BlackList("Initial reason");

        // Act
        guest.ChangeBlackListedReason("Updated reason");

        // Assert
        guest.BlackListedReason.Should().Be("Updated reason");
    }

    [Fact]
    public void EditNotes_ShouldUpdateNotes()
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        guest.EditNotes("Requires airport pickup");

        // Assert
        guest.Notes.Should().Be("Requires airport pickup");
    }

    private static Guest CreateGuest()
    {
        return new Guest(
            Guid.NewGuid(),
            "John",
            "Doe",
            GuestDocumentType.IdCard,
            "00112345678",
            CreatePhoneNumber(),
            CurrencyCode.DOP,
            true,
            "United States",
            CreateRnc(),
            CreateEmail(),
            "English",
            "Frequent guest");
    }

    private static Rnc CreateRnc()
    {
        return new Rnc("123456789");
    }

    private static Email CreateEmail()
    {
        return new Email("guest@example.com");
    }

    private static PhoneNumber CreatePhoneNumber()
    {
        return new PhoneNumber("+1 809 555 1234");
    }
}
