using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelContactInfo;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Application.UseCases.Hotels.Commands.UpdateHotelContactInfo;

public class UpdateHotelContactInfoCommandValidatorTests
{
  private readonly UpdateHotelContactInfoCommandValidator _validator;

  public UpdateHotelContactInfoCommandValidatorTests()
  {
    _validator = new UpdateHotelContactInfoCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = CreateUpdateHotelContactInfoCommand(Guid.Empty, string.Empty, string.Empty);

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.HotelId);
    result.ShouldHaveValidationErrorFor(x => x.BusinessEmail);
    result.ShouldHaveValidationErrorFor(x => x.BusinessPhoneNumber);
  }

  [Fact]
  public void Validator_HasErrors_WhenFieldsExceedMaxLength()
  {
    // Arrange
    var command = CreateUpdateHotelContactInfoCommand(
        Guid.NewGuid(),
        $"{new string('a', 255)}@test.com",
        new string('1', 26));

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.BusinessEmail);
    result.ShouldHaveValidationErrorFor(x => x.BusinessPhoneNumber);
  }

  [Theory]
  [InlineData("invalid-email")]
  [InlineData("test.com")]
  public void Validator_HasError_WhenBusinessEmailIsInvalid(string invalidBusinessEmail)
  {
    // Arrange
    var command = CreateUpdateHotelContactInfoCommand(
        Guid.NewGuid(),
        invalidBusinessEmail,
        CreateBusinessPhoneNumber());

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.BusinessEmail);
  }

  [Theory]
  [InlineData("invalid-phone")]
  [InlineData("a1235678910")]
  public void Validator_HasError_WhenBusinessPhoneNumberIsInvalid(string invalidBusinessPhoneNumber)
  {
    // Arrange
    var command = CreateUpdateHotelContactInfoCommand(
        Guid.NewGuid(),
        CreateBusinessEmail(),
        invalidBusinessPhoneNumber);

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.BusinessPhoneNumber);
  }

  // Helper methods
  private static UpdateHotelContactInfoCommand CreateUpdateHotelContactInfoCommand(
      Guid hotelId,
      string businessEmail,
      string businessPhoneNumber)
  {
    return new UpdateHotelContactInfoCommand
    {
      HotelId = hotelId,
      BusinessEmail = businessEmail,
      BusinessPhoneNumber = businessPhoneNumber
    };
  }

  private static string CreateBusinessEmail() => "hotel@brisa.com";

  private static string CreateBusinessPhoneNumber() => "+18295554321";
}
