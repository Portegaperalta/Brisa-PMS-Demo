using BrisaPMS.Application.UseCases.Guests.Commands.MakeGuestVip;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Guests.Commands.MakeGuestVip;

public class MakeGuestVipCommandValidatorTests
{
    private readonly MakeGuestVipCommandValidator _validator;

    public MakeGuestVipCommandValidatorTests()
    {
        _validator = new MakeGuestVipCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new MakeGuestVipCommand { GuestId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GuestId);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new MakeGuestVipCommand { GuestId = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
