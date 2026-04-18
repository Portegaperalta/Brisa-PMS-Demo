using BrisaPMS.Application.UseCases.Guests.Commands.CreateGuest;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Guests.Commands.CreateGuest;

public class CreateGuestCommandValidatorTests
{
    private readonly CreateGuestCommandValidator _validator;

    public CreateGuestCommandValidatorTests()
    {
        _validator = new CreateGuestCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateCommand(
            Guid.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            null,
            null,
            string.Empty,
            string.Empty,
            string.Empty,
            null,
            default,
            null);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.HotelId);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
        result.ShouldHaveValidationErrorFor(x => x.DocumentType);
        result.ShouldHaveValidationErrorFor(x => x.DocumentNumber);
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
        result.ShouldHaveValidationErrorFor(x => x.PreferredCurrency);
        result.ShouldHaveValidationErrorFor(x => x.IsVip);
    }

    [Fact]
    public void Validator_HasErrors_WhenFieldsExceedMaxLength()
    {
        // Arrange
        var command = CreateCommand(
            Guid.NewGuid(),
            new string('F', 251),
            new string('L', 251),
            new string('D', 51),
            new string('N', 251),
            new string('C', 101),
            "123456789",
            new string('E', 255) + "@test.com",
            new string('9', 26),
            "USDD",
            new string('P', 51),
            true,
            new string('T', 501));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
        result.ShouldHaveValidationErrorFor(x => x.DocumentType);
        result.ShouldHaveValidationErrorFor(x => x.DocumentNumber);
        result.ShouldHaveValidationErrorFor(x => x.Country);
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
        result.ShouldHaveValidationErrorFor(x => x.PreferredCurrency);
        result.ShouldHaveValidationErrorFor(x => x.PreferredLanguage);
        result.ShouldHaveValidationErrorFor(x => x.Notes);
    }

    [Theory]
    [InlineData("Invalid", "guest@example.com", "+18095551234", "USD", "123456789", true)]
    [InlineData("Passport", "invalid-email", "+18095551234", "USD", "123456789", true)]
    [InlineData("Passport", "guest@example.com", "invalid-phone", "USD", "123456789", true)]
    [InlineData("Passport", "guest@example.com", "+18095551234", "XYZ", "123456789", true)]
    [InlineData("Passport", "guest@example.com", "+18095551234", "USD", "ABC123456", true)]
    public void Validator_HasErrors_WhenFormattedFieldsAreInvalid(
        string documentType,
        string email,
        string phoneNumber,
        string preferredCurrency,
        string? rnc,
        bool isVip)
    {
        // Arrange
        var command = CreateCommand(
            Guid.NewGuid(),
            "John",
            "Doe",
            documentType,
            "A1234567",
            "Dominican Republic",
            rnc,
            email,
            phoneNumber,
            preferredCurrency,
            "Spanish",
            isVip,
            "Frequent guest");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        if (documentType == "Invalid")
            result.ShouldHaveValidationErrorFor(x => x.DocumentType);

        if (email == "invalid-email")
            result.ShouldHaveValidationErrorFor(x => x.Email);

        if (phoneNumber == "invalid-phone")
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);

        if (preferredCurrency == "XYZ")
            result.ShouldHaveValidationErrorFor(x => x.PreferredCurrency);

        if (rnc == "ABC123456")
            result.ShouldHaveValidationErrorFor(x => x.Rnc);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static CreateGuestCommand CreateValidCommand()
    {
        return CreateCommand(
            Guid.NewGuid(),
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
            "Frequent guest");
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
