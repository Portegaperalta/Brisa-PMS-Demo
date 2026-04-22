using BrisaPMS.Application.UseCases.Users.Commands.CreateUser;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserCommandValidatorTests
{
  private readonly CreateUserCommandValidator _validator;

  public CreateUserCommandValidatorTests()
  {
    _validator = new CreateUserCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = CreateCommand(
        string.Empty,
        null,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        null,
        string.Empty);

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Role);
    result.ShouldHaveValidationErrorFor(x => x.FirstName);
    result.ShouldHaveValidationErrorFor(x => x.LastName);
    result.ShouldHaveValidationErrorFor(x => x.Email);
    result.ShouldHaveValidationErrorFor(x => x.Password);
    result.ShouldHaveValidationErrorFor(x => x.PreferredLanguage);
  }

  [Fact]
  public void Validator_HasErrors_WhenFieldsExceedMaxLength()
  {
    // Arrange
    var command = CreateCommand(
        "Admin",
        Guid.NewGuid(),
        new string('F', 251),
        new string('L', 251),
        new string('E', 255) + "@test.com",
        new string('P', 513),
        new string('9', 26),
        "En");

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.FirstName);
    result.ShouldHaveValidationErrorFor(x => x.LastName);
    result.ShouldHaveValidationErrorFor(x => x.Email);
    result.ShouldHaveValidationErrorFor(x => x.Password);
    result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
  }

  [Theory]
  [InlineData("InvalidRole", "Test@1234", "test@example.com", "+18095551234", "En")]
  [InlineData("Admin", "test1234", "test@example.com", "+18095551234", "En")]
  [InlineData("Admin", "Testpass", "test@example.com", "+18095551234", "En")]
  [InlineData("Admin", "TEST1234", "test@example.com", "+18095551234", "En")]
  [InlineData("Admin", "Test@1234", "invalid-email", "+18095551234", "En")]
  [InlineData("Admin", "Test@1234", "test@example.com", "invalid-phone", "En")]
  [InlineData("Admin", "Test@1234", "test@example.com", "+18095551234", "InvalidLanguage")]
  public void Validator_HasErrors_WhenFormattedFieldsAreInvalid(
      string role,
      string password,
      string email,
      string phoneNumber,
      string preferredLanguage)
  {
    // Arrange
    var command = CreateCommand(
        role,
        Guid.NewGuid(),
        "John",
        "Doe",
        email,
        password,
        phoneNumber,
        preferredLanguage);

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    if (role == "InvalidRole")
      result.ShouldHaveValidationErrorFor(x => x.Role);

    if (password == "test1234" || password == "Testpass" || password == "TEST1234")
      result.ShouldHaveValidationErrorFor(x => x.Password);

    if (email == "invalid-email")
      result.ShouldHaveValidationErrorFor(x => x.Email);

    if (phoneNumber == "invalid-phone")
      result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);

    if (preferredLanguage == "InvalidLanguage")
      result.ShouldHaveValidationErrorFor(x => x.PreferredLanguage);
  }

  [Fact]
  public void Validator_HasNoErrors_WhenCommandIsValid()
  {
    // Arrange
    var command = CreateValidCommand();

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Fact]
  public void Validator_HasNoErrors_WhenPhoneNumberIsNull()
  {
    // Arrange
    var command = CreateCommand(
        "Admin",
        Guid.NewGuid(),
        "John",
        "Doe",
        "test@example.com",
        "Test@1234",
        null,
        "English");

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
  }

  [Fact]
  public void Validator_HasNoErrors_WhenHotelIdIsNull()
  {
    // Arrange
    var command = CreateCommand(
        "Admin",
        null,
        "John",
        "Doe",
        "test@example.com",
        "Test@1234",
        "+18095551234",
        "English");

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldNotHaveValidationErrorFor(x => x.HotelId);
  }

  private static CreateUserCommand CreateValidCommand()
  {
    return CreateCommand(
        "Admin",
        Guid.NewGuid(),
        "John",
        "Doe",
        "test@example.com",
        "Test@1234",
        "+18095551234",
        "En");
  }

  private static CreateUserCommand CreateCommand(
      string role,
      Guid? hotelId,
      string firstName,
      string lastName,
      string email,
      string password,
      string? phoneNumber,
      string preferredLanguage)
  {
    return new CreateUserCommand
    {
      Role = role,
      HotelId = hotelId,
      FirstName = firstName,
      LastName = lastName,
      Email = email,
      Password = password,
      PhoneNumber = phoneNumber,
      PreferredLanguage = preferredLanguage
    };
  }
}