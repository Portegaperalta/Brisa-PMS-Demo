using BrisaPMS.Application.UseCases.Guests.Commands.BlacklistGuest;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Guests.Commands.BlacklistGuest;

public class BlacklistGuestCommandValidatorTests
{
    private readonly BlacklistGuestCommandValidator _validator;

    public BlacklistGuestCommandValidatorTests()
    {
        _validator = new BlacklistGuestCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateCommand(Guid.Empty, string.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GuestId);
        result.ShouldHaveValidationErrorFor(x => x.BlacklistedReason);
    }

    [Fact]
    public void Validator_HasError_WhenReasonExceedsMaxLength()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), new string('R', 501));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BlacklistedReason);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), "Repeated property damage");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static BlacklistGuestCommand CreateCommand(Guid guestId, string blacklistedReason)
    {
        return new BlacklistGuestCommand
        {
            GuestId = guestId,
            BlacklistedReason = blacklistedReason
        };
    }
}
