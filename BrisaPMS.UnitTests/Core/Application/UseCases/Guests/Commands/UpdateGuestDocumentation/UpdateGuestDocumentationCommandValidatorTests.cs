using BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestDocumentation;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Guests.Commands.UpdateGuestDocumentation;

public class UpdateGuestDocumentationCommandValidatorTests
{
    private readonly UpdateGuestDocumentationCommandValidator _validator;

    public UpdateGuestDocumentationCommandValidatorTests()
    {
        _validator = new UpdateGuestDocumentationCommandValidator();
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
        result.ShouldHaveValidationErrorFor(x => x.DocumentType);
        result.ShouldHaveValidationErrorFor(x => x.DocumentNumber);
    }

    [Fact]
    public void Validator_HasErrors_WhenFieldsExceedMaxLength()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), new string('D', 21), new string('N', 251));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DocumentType);
        result.ShouldHaveValidationErrorFor(x => x.DocumentNumber);
    }

    [Fact]
    public void Validator_HasError_WhenDocumentTypeIsNotSupported()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), "Invalid", "A1234567");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DocumentType);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), "Passport", "A1234567");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
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
}
