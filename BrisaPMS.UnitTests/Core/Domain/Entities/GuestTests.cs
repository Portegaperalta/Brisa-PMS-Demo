using BrisaPMS.Domain.Entities;
using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Entities;

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
            DocumentType.Passport,
            "A1234567",
            CurrencyCode.USD,
            true,
            "United States",
            rnc,
            email,
            phoneNumber,
            "English",
            "VIP guest");

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
        result.HotelId.Should().Be(hotelId);
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.DocumentType.Should().Be(DocumentType.Passport);
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
        result.UpdatedAt.Should().BeNull();
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Constructor_ShouldCreateGuest_WhenOptionalValuesAreNotProvided()
    {
        // Arrange
        var hotelId = Guid.NewGuid();

        // Act
        var result = new Guest(
            hotelId,
            "John",
            "Doe",
            DocumentType.IdCard,
            "00112345678",
            CurrencyCode.DOP,
            false);

        // Assert
        result.Country.Should().BeNull();
        result.Rnc.Should().BeNull();
        result.Email.Should().BeNull();
        result.PhoneNumber.Should().BeNull();
        result.PreferredLanguage.Should().BeNull();
        result.Notes.Should().BeNull();
        result.IsVip.Should().BeFalse();
    }

    [Theory]
    [InlineData("First Name")]
    [InlineData("Last Name")]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenRequiredNameIsWhiteSpace(string fieldName)
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";

        if (fieldName == "First Name")
            firstName = " ";
        else
            lastName = " ";

        // Act
        Action act = () => _ = new Guest(Guid.NewGuid(), firstName, lastName, DocumentType.IdCard, "00112345678", CurrencyCode.DOP, false);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenDocumentTypeIsInvalid()
    {
        // Arrange
        var invalidDocumentType = (DocumentType)999;

        // Act
        Action act = () => _ = new Guest(Guid.NewGuid(), "John", "Doe", invalidDocumentType, "00112345678", CurrencyCode.DOP, false);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenDocumentNumberIsEmpty()
    {
        // Arrange
        const string documentNumber = " ";

        // Act
        Action act = () => _ = new Guest(Guid.NewGuid(), "John", "Doe", DocumentType.IdCard, documentNumber, CurrencyCode.DOP, false);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenPreferredCurrencyIsInvalid()
    {
        // Arrange
        var invalidCurrency = (CurrencyCode)999;

        // Act
        Action act = () => _ = new Guest(Guid.NewGuid(), "John", "Doe", DocumentType.IdCard, "00112345678", invalidCurrency, false);

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

    [Fact]
    public void ChangeFirstName_ShouldThrowEmptyRequiredFieldException_WhenValueIsWhiteSpace()
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        Action act = () => guest.ChangeFirstName(" ");

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
        guest.ChangeDocumentType(DocumentType.Passport);

        // Assert
        guest.DocumentType.Should().Be(DocumentType.Passport);
    }

    [Fact]
    public void ChangeDocumentType_ShouldThrowBusinessRuleException_WhenValueIsInvalid()
    {
        // Arrange
        var guest = CreateGuest();
        var invalidDocumentType = (DocumentType)999;

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

    [Fact]
    public void ChangeCountry_ShouldThrowEmptyRequiredFieldException_WhenValueIsWhiteSpace()
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        Action act = () => guest.ChangeCountry(" ");

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void ChangeContactPreferences_ShouldUpdateReferenceAndPreferenceProperties()
    {
        // Arrange
        var guest = CreateGuest();
        var newRnc = new Rnc("00112345678");
        var newEmail = new Email("guest.updated@example.com");
        var newPhoneNumber = new PhoneNumber("+1 829 555 4321");

        // Act
        guest.ChangeRnc(newRnc);
        guest.ChangeEmail(newEmail);
        guest.ChangePhoneNumber(newPhoneNumber);
        guest.ChangePreferredCurrency(CurrencyCode.EUR);
        guest.ChangePreferredLanguage("Spanish");

        // Assert
        guest.Rnc.Should().Be(newRnc);
        guest.Email.Should().Be(newEmail);
        guest.PhoneNumber.Should().Be(newPhoneNumber);
        guest.PreferredCurrency.Should().Be(CurrencyCode.EUR);
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

    [Fact]
    public void ChangePreferredLanguage_ShouldThrowEmptyRequiredFieldException_WhenValueIsWhiteSpace()
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        Action act = () => guest.ChangePreferredLanguage(" ");

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void VipMethods_ShouldToggleVipStatus()
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        guest.DisableVip();
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

    [Fact]
    public void BlackList_ShouldThrowBusinessRuleException_WhenReasonIsWhiteSpace()
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        Action act = () => guest.BlackList(" ");

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void DisableBlackListAndChangeBlackListedReason_ShouldUpdateBlackListProperties()
    {
        // Arrange
        var guest = CreateGuest();
        guest.BlackList("Initial reason");

        // Act
        guest.DisableBlackList();
        guest.ChangeBlackListedReason("Updated reason");

        // Assert
        guest.IsBlackListed.Should().BeFalse();
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

    [Fact]
    public void UpdateLastUpdatedTime_ShouldSetUpdatedAt()
    {
        // Arrange
        var guest = CreateGuest();

        // Act
        guest.UpdateLastUpdatedTime();

        // Assert
        guest.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    private static Guest CreateGuest()
    {
        return new Guest(
            Guid.NewGuid(),
            "John",
            "Doe",
            DocumentType.IdCard,
            "00112345678",
            CurrencyCode.DOP,
            true,
            "United States",
            CreateRnc(),
            CreateEmail(),
            CreatePhoneNumber(),
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
