using BrisaPMS.Application.UseCases.Guests.Commands.RevokeGuestVip;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Guests.Commands.RevokeGuestVip;

public class RevokeGuestVipCommandValidatorTests
{
    private readonly RevokeGuestVipCommandValidator _validator;

    public RevokeGuestVipCommandValidatorTests()
    {
        _validator = new RevokeGuestVipCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new RevokeGuestVipCommand { GuestId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GuestId);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new RevokeGuestVipCommand { GuestId = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
