using BrisaPMS.Application.UseCases.Stays.Commands.CompleteStay;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Stays.Commands.CompleteStay;

public class CompleteStayCommandValidatorTests
{
    private readonly CompleteStayCommandValidator _validator;

    public CompleteStayCommandValidatorTests()
    {
        _validator = new CompleteStayCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new CompleteStayCommand { StayId = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StayId);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new CompleteStayCommand { StayId = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
