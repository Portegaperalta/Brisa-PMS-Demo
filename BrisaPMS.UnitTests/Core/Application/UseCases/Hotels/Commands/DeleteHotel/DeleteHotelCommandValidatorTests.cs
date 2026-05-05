using BrisaPMS.Application.UseCases.Hotels.Commands.DeleteHotel;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Hotels.Commands.DeleteHotel;

public class DeleteHotelCommandValidatorTests
{
    private readonly DeleteHotelCommandValidator _validator;

    public DeleteHotelCommandValidatorTests()
    {
        _validator = new DeleteHotelCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = new DeleteHotelCommand { Id = Guid.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Validator_HasNoErrors_WhenCommandIsValid()
    {
        // Arrange
        var command = new DeleteHotelCommand { Id = Guid.NewGuid() };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}