using BrisaPMS.Application.UseCases.Hotels.Commands.CreateHotel;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Hotels.Commands.CreateHotel;

public class CreateHotelCommandValidatorTests
{
  private readonly CreateHotelCommandValidator _commandValidator;

  public CreateHotelCommandValidatorTests()
  {
    _commandValidator = new CreateHotelCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = CreateCreateHotelCommand(
        string.Empty,
        string.Empty,
        CreateLogoUrl(),
        string.Empty,
        string.Empty,
        string.Empty,
        CreateAddress2(),
        string.Empty,
        string.Empty,
        string.Empty,
        default,
        default,
        CreateDefaultCurrencyCode(),
        default,
        default);

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.LegalName);
    result.ShouldHaveValidationErrorFor(x => x.CommercialName);
    result.ShouldHaveValidationErrorFor(x => x.BusinessEmail);
    result.ShouldHaveValidationErrorFor(x => x.BusinessPhoneNumber);
    result.ShouldHaveValidationErrorFor(x => x.Address1);
    result.ShouldHaveValidationErrorFor(x => x.City);
    result.ShouldHaveValidationErrorFor(x => x.Province);
    result.ShouldHaveValidationErrorFor(x => x.ZipCode);
    result.ShouldHaveValidationErrorFor(x => x.CheckInTime);
    result.ShouldHaveValidationErrorFor(x => x.CheckOutTime);
    result.ShouldHaveValidationErrorFor(x => x.ItbisRate);
    result.ShouldHaveValidationErrorFor(x => x.ServiceChargeRate);
  }

  [Fact]
  public void Validator_HasErrors_WhenFieldsExceedMaxLength()
  {
    // Arrange
    var command = CreateCreateHotelCommand(
        new string('L', 251),
        new string('C', 251),
        $"https://example.com/{new string('L', 2041)}",
        $"{new string('a', 246)}@test.com",
        new string('1', 26),
        new string('A', 201),
        new string('B', 201),
        new string('C', 101),
        new string('P', 101),
        new string('1', 11),
        CreateCheckInTime(),
        CreateCheckOutTime(),
        "USDD",
        CreateItbisRate(),
        CreateServiceChargeRate());

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.LegalName);
    result.ShouldHaveValidationErrorFor(x => x.CommercialName);
    result.ShouldHaveValidationErrorFor(x => x.LogoUrl);
    result.ShouldHaveValidationErrorFor(x => x.BusinessEmail);
    result.ShouldHaveValidationErrorFor(x => x.BusinessPhoneNumber);
    result.ShouldHaveValidationErrorFor(x => x.Address1);
    result.ShouldHaveValidationErrorFor(x => x.Address2);
    result.ShouldHaveValidationErrorFor(x => x.City);
    result.ShouldHaveValidationErrorFor(x => x.Province);
    result.ShouldHaveValidationErrorFor(x => x.ZipCode);
    result.ShouldHaveValidationErrorFor(x => x.DefaultCurrencyCode);
  }

  [Theory]
  [InlineData("invalid-email")]
  [InlineData("test.com")]
  public void Validator_HasError_WhenBusinessEmailIsInvalid(string invalidEmail)
  {
    // Arrange
    var command = CreateValidCommand();
    command.BusinessEmail = invalidEmail;

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.BusinessEmail);
  }

  [Theory]
  [InlineData("invalid-phone")]
  [InlineData("a1235678910")]
  public void Validator_HasError_WhenBusinessPhoneNumberIsInvalid(string invalidPhoneNumber)
  {
    // Arrange
    var command = CreateValidCommand();
    command.BusinessPhoneNumber = invalidPhoneNumber;

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.BusinessPhoneNumber);
  }

  [Theory]
  [InlineData("12A45")]
  [InlineData("@12345")]
  public void Validator_HasError_WhenZipCodeIsInvalid(string invalidZipCode)
  {
    // Arrange
    var command = CreateValidCommand();
    command.ZipCode = invalidZipCode;

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.ZipCode);
  }

  [Theory]
  [InlineData("ZZZ")]
  [InlineData("ABC")]
  public void Validator_HasError_WhenDefaultCurrencyCodeIsInvalid(string invalidCurrencyCode)
  {
    // Arrange
    var command = CreateValidCommand();
    command.DefaultCurrencyCode = invalidCurrencyCode;

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.DefaultCurrencyCode);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(101)]
  public void Validator_HasError_WhenItbisRateIsOutOfRange(decimal invalidItbisRate)
  {
    // Arrange
    var command = CreateValidCommand();
    command.ItbisRate = invalidItbisRate;

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.ItbisRate);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(101)]
  public void Validator_HasError_WhenServiceChargeRateIsOutOfRange(decimal invalidServiceChargeRate)
  {
    // Arrange
    var command = CreateValidCommand();
    command.ServiceChargeRate = invalidServiceChargeRate;

    // Act
    var result = _commandValidator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.ServiceChargeRate);
  }

  // Helper methods
  private static CreateHotelCommand CreateValidCommand()
  {
    return CreateCreateHotelCommand(
        CreateLegalName(),
        CreateCommercialName(),
        CreateLogoUrl(),
        CreateBusinessEmail(),
        CreateBusinessPhoneNumber(),
        CreateAddress1(),
        CreateAddress2(),
        CreateCity(),
        CreateProvince(),
        CreateZipCode(),
        CreateCheckInTime(),
        CreateCheckOutTime(),
        CreateDefaultCurrencyCode(),
        CreateItbisRate(),
        CreateServiceChargeRate());
  }

  private static CreateHotelCommand CreateCreateHotelCommand(
      string legalName,
      string commercialName,
      string? logoUrl,
      string businessEmail,
      string businessPhoneNumber,
      string address1,
      string? address2,
      string city,
      string province,
      string zipCode,
      TimeOnly checkInTime,
      TimeOnly checkOutTime,
      string defaultCurrencyCode,
      decimal itbisRate,
      decimal serviceChargeRate)
  {
    return new CreateHotelCommand
    {
      LegalName = legalName,
      CommercialName = commercialName,
      LogoUrl = logoUrl,
      BusinessEmail = businessEmail,
      BusinessPhoneNumber = businessPhoneNumber,
      Address1 = address1,
      Address2 = address2,
      City = city,
      Province = province,
      ZipCode = zipCode,
      CheckInTime = checkInTime,
      CheckOutTime = checkOutTime,
      DefaultCurrencyCode = defaultCurrencyCode,
      ItbisRate = itbisRate,
      ServiceChargeRate = serviceChargeRate
    };
  }

  private static string CreateLegalName() => "Brisa S.R.L";
  private static string CreateCommercialName() => "Brisa Hotel";
  private static string CreateLogoUrl() => "https://testlogourl.jpg";
  private static string CreateBusinessEmail() => "brisaHotel@test.com";
  private static string CreateBusinessPhoneNumber() => "1234567891";
  private static string CreateAddress1() => "Address 1";
  private static string CreateAddress2() => "Address 2";
  private static string CreateCity() => "test city";
  private static string CreateProvince() => "test province";
  private static string CreateZipCode() => "12345";
  private static TimeOnly CreateCheckInTime() => new(12, 0, 0);
  private static TimeOnly CreateCheckOutTime() => new(17, 0, 0);
  private static string CreateDefaultCurrencyCode() => "USD";
  private static decimal CreateItbisRate() => 0.18m;
  private static decimal CreateServiceChargeRate() => 0.10m;
}
