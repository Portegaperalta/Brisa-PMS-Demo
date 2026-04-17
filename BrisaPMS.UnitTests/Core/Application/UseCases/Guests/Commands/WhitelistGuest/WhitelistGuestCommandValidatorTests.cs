using BrisaPMS.Application.UseCases.Guests.Commands.WhitelistGuest;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Guests.Commands.WhitelistGuest;

public class WhitelistGuestCommandValidatorTests
{
    private readonly WhitelistGuestCommandValidator _validator;

    public WhitelistGuestCommandValidatorTests()
    {
        _validator = new WhitelistGuestCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new WhitelistGuestCommand { GuestId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GuestId);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new WhitelistGuestCommand { GuestId = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
