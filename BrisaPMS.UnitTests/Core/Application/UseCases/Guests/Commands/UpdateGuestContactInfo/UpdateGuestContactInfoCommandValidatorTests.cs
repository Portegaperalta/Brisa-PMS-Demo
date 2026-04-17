using BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestContactInfo;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Guests.Commands.UpdateGuestContactInfo;

public class UpdateGuestContactInfoCommandValidatorTests
{
    private readonly UpdateGuestContactInfoCommandValidator _validator;

    public UpdateGuestContactInfoCommandValidatorTests()
    {
        _validator = new UpdateGuestContactInfoCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateCommand(Guid.Empty, string.Empty, string.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GuestId);
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Fact]
    public void Validator_HasErrors_WhenFieldsExceedMaxLength()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), new string('E', 255) + "@test.com", new string('9', 26));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Theory]
    [InlineData("invalid-email", "+18095551234")]
    [InlineData("guest@example.com", "invalid-phone")]
    public void Validator_HasErrors_WhenFormattedFieldsAreInvalid(string email, string phoneNumber)
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), email, phoneNumber);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        if (email == "invalid-email")
            result.ShouldHaveValidationErrorFor(x => x.Email);

        if (phoneNumber == "invalid-phone")
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), "guest.updated@example.com", "+18095550000");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static UpdateGuestContactInfoCommand CreateCommand(Guid guestId, string email, string phoneNumber)
    {
        return new UpdateGuestContactInfoCommand
        {
            GuestId = guestId,
            Email = email,
            PhoneNumber = phoneNumber
        };
    }
}
