using BrisaPMS.Application.UseCases.Amenities.Commands.DeleteAmenity;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Amenities.Commands.DeleteAmenity;

public class DeleteAmenityCommandValidatorTests
{
  private readonly DeleteAmenityCommandValidator _validator;

  public DeleteAmenityCommandValidatorTests()
  {
    _validator = new DeleteAmenityCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = new DeleteAmenityCommand { Id = Guid.Empty };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Id);
  }

  [Fact]
  public void Validator_HasNoErrors_WhenCommandIsValid()
  {
    // Arrange
    var command = new DeleteAmenityCommand { Id = Guid.NewGuid() };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }
}