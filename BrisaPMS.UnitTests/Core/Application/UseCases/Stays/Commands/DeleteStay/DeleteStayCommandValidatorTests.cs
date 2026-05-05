using BrisaPMS.Application.UseCases.Stays.Commands.DeleteStay;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Stays.Commands.DeleteStay;

public class DeleteStayCommandValidatorTests
{
    private readonly DeleteStayCommandValidator _validator;

    public DeleteStayCommandValidatorTests()
    {
        _validator = new DeleteStayCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new DeleteStayCommand { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new DeleteStayCommand { Id = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
