using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelDefaultCurrency;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Hotels.Commands.UpdateHotelDefaultCurrency;

public class UpdateHotelDefaultCurrencyCommandValidatorTests
{
  private readonly UpdateHotelDefaultCurrencyCommandValidator _validator;

  public UpdateHotelDefaultCurrencyCommandValidatorTests()
  {
    _validator = new UpdateHotelDefaultCurrencyCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = CreateUpdateHotelDefaultCurrencyCommand(Guid.Empty, string.Empty);

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.HotelId);
    result.ShouldHaveValidationErrorFor(x => x.DefaultCurrencyCode);
  }

  [Fact]
  public void Validator_HasErrors_WhenFieldsExceedMaxLength()
  {
    // Arrange
    var command = CreateUpdateHotelDefaultCurrencyCommand(Guid.NewGuid(), "USDD");

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.DefaultCurrencyCode);
  }

  [Theory]
  [InlineData("ZZZ")]
  [InlineData("ABC")]
  public void Validator_HasError_WhenDefaultCurrencyCodeIsInvalid(string invalidCurrencyCode)
  {
    // Arrange
    var command = CreateUpdateHotelDefaultCurrencyCommand(Guid.NewGuid(), invalidCurrencyCode);

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.DefaultCurrencyCode);
  }

  // Helper methods
  private static UpdateHotelDefaultCurrencyCommand CreateUpdateHotelDefaultCurrencyCommand(
      Guid hotelId,
      string defaultCurrencyCode)
  {
    return new UpdateHotelDefaultCurrencyCommand
    {
      HotelId = hotelId,
      DefaultCurrencyCode = defaultCurrencyCode
    };
  }
}
