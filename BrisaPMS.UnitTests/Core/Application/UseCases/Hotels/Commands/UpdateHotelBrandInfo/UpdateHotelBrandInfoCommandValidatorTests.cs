using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelBrandInfo;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Hotels.Commands.UpdateHotelBrandInfo;

public class UpdateHotelBrandInfoCommandValidatorTests
{
  private readonly UpdateHotelBrandInfoCommandValidator _commandValidator;

  public UpdateHotelBrandInfoCommandValidatorTests()
  {
    _commandValidator = new UpdateHotelBrandInfoCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = CreateUpdateHotelBrandInfoCommand(Guid.Empty, string.Empty, string.Empty, CreateLogoUrl());

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.HotelId);
    result.ShouldHaveValidationErrorFor(x => x.LegalName);
    result.ShouldHaveValidationErrorFor(x => x.CommercialName);
  }

  [Fact]
  public void Validator_HasErrors_WhenFieldsExceedMaxLength()
  {
    // Arrange
    var command = CreateUpdateHotelBrandInfoCommand(
        Guid.NewGuid(),
        new string('L', 251),
        new string('C', 251),
        $"https://example.com/{new string('L', 2041)}");

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.LegalName);
    result.ShouldHaveValidationErrorFor(x => x.CommercialName);
    result.ShouldHaveValidationErrorFor(x => x.LogoUrl);
  }

  [Theory]
  [InlineData("newlogo.png")]
  [InlineData("ftp://example.com/logo.png")]
  public void Validator_HasError_WhenLogoUrlIsInvalid(string invalidLogoUrl)
  {
    // Arrange
    var command = CreateUpdateHotelBrandInfoCommand(
        Guid.NewGuid(),
        CreateLegalName(),
        CreateCommercialName(),
        invalidLogoUrl);

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.LogoUrl);
  }

  // Helper methods
  private static UpdateHotelBrandInfoCommand CreateUpdateHotelBrandInfoCommand(
      Guid hotelId,
      string legalName,
      string commercialName,
      string? logoUrl)
  {
    return new UpdateHotelBrandInfoCommand
    {
      HotelId = hotelId,
      LegalName = legalName,
      CommercialName = commercialName,
      LogoUrl = logoUrl
    };
  }

  private static string CreateLegalName() => "Brisa Hospitality SRL";

  private static string CreateCommercialName() => "Hotel Brisa";

  private static string CreateLogoUrl() => "https://example.com/logo.png";
}
