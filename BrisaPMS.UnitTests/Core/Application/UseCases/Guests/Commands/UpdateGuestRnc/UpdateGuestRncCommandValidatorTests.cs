using BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestRnc;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Guests.Commands.UpdateGuestRnc;

public class UpdateGuestRncCommandValidatorTests
{
    private readonly UpdateGuestRncCommandValidator _validator;

    public UpdateGuestRncCommandValidatorTests()
    {
        _validator = new UpdateGuestRncCommandValidator();
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
        result.ShouldHaveValidationErrorFor(x => x.Rnc);
    }

    [Theory]
    [InlineData("12345678")]
    [InlineData("123456789012")]
    [InlineData("ABC123456")]
    public void Validator_HasErrors_WhenRncIsInvalid(string rnc)
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), rnc);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Rnc);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), "123456789");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static UpdateGuestRncCommand CreateCommand(Guid guestId, string rnc)
    {
        return new UpdateGuestRncCommand
        {
            GuestId = guestId,
            Rnc = rnc
        };
    }
}
