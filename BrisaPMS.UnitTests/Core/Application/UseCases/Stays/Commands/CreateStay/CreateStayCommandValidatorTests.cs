using BrisaPMS.Application.UseCases.Stays.Commands.CreateStay;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Stays.Commands.CreateStay;

public class CreateStayCommandValidatorTests
{
    private readonly CreateStayCommandValidator _validator;

    public CreateStayCommandValidatorTests()
    {
        _validator = new CreateStayCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new CreateStayCommand
        {
            GuestId = Guid.Empty,
            BookingId = Guid.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.GuestId);
        result.ShouldHaveValidationErrorFor(x => x.BookingId);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateStayCommand
        {
            GuestId = Guid.NewGuid(),
            BookingId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
