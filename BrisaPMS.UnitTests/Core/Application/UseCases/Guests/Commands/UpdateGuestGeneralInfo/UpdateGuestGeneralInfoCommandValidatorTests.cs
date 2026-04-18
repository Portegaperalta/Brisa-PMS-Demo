using BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestGeneralInfo;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Guests.Commands.UpdateGuestGeneralInfo;

public class UpdateGuestGeneralInfoCommandValidatorTests
{
    private readonly UpdateGuestGeneralInfoCommandValidator _validator;

    public UpdateGuestGeneralInfoCommandValidatorTests()
    {
        _validator = new UpdateGuestGeneralInfoCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateCommand(Guid.Empty, string.Empty, string.Empty, null, null, null);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GuestId);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void Validator_HasErrors_WhenFieldsExceedMaxLength()
    {
        // Arrange
        var command = CreateCommand(
            Guid.NewGuid(),
            new string('F', 251),
            new string('L', 251),
            new string('C', 101),
            new string('P', 51),
            new string('N', 501));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
        result.ShouldHaveValidationErrorFor(x => x.Country);
        result.ShouldHaveValidationErrorFor(x => x.PreferredLanguage);
        result.ShouldHaveValidationErrorFor(x => x.Notes);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), "John", "Doe", "Dominican Republic", "English", "Frequent guest");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
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
}
