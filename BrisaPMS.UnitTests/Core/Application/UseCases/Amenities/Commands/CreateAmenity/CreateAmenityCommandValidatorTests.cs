using BrisaPMS.Application.UseCases.Amenities.Commands.CreateAmenity;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Amenities.Commands.CreateAmenity;

public class CreateAmenityCommandValidatorTests
{
  private readonly CreateAmenityCommandValidator _validator;

  public CreateAmenityCommandValidatorTests()
  {
    _validator = new CreateAmenityCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = CreateCommand(string.Empty, string.Empty, false);

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Name);
    result.ShouldHaveValidationErrorFor(x => x.Description);
  }

  [Fact]
  public void Validator_HasErrors_WhenFieldsExceedMaxLength()
  {
    // Arrange
    var command = CreateCommand(new string('N', 101), new string('D', 501), true);

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Name);
    result.ShouldHaveValidationErrorFor(x => x.Description);
  }

  [Fact]
  public void Validator_HasNoErrors_WhenCommandIsValid()
  {
    // Arrange
    var command = CreateCommand("Pool Access", "Access to the swimming pool", true);

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  private static CreateAmenityCommand CreateCommand(string name, string description, bool isActive)
  {
    return new CreateAmenityCommand
    {
      Name = name,
      Description = description,
      IsActive = isActive
    };
  }

}
