using BrisaPMS.Application.UseCases.Users.Commands.ChangePreferredLanguage;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Users.Commands.ChangePreferredLanguage;

public class ChangePreferredLanguageCommandValidatorTests
{
  private readonly ChangePreferredLanguageCommandValidator _validator;

  public ChangePreferredLanguageCommandValidatorTests()
  {
    _validator = new ChangePreferredLanguageCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenUserIdIsEmpty()
  {
    var command = new ChangePreferredLanguageCommand
    {
      UserId = Guid.Empty,
      PreferredLanguage = "En"
    };

    var result = _validator.TestValidate(command);

    result.ShouldHaveValidationErrorFor(x => x.UserId);
  }

  [Fact]
  public void Validator_HasErrors_WhenPreferredLanguageIsEmpty()
  {
    var command = new ChangePreferredLanguageCommand
    {
      UserId = Guid.NewGuid(),
      PreferredLanguage = string.Empty
    };

    var result = _validator.TestValidate(command);

    result.ShouldHaveValidationErrorFor(x => x.PreferredLanguage);
  }

  [Fact]
  public void Validator_HasErrors_WhenPreferredLanguageIsNotAValidEnumValue()
  {
    var command = new ChangePreferredLanguageCommand
    {
      UserId = Guid.NewGuid(),
      PreferredLanguage = "Invalid"
    };

    var result = _validator.TestValidate(command);

    result.ShouldHaveValidationErrorFor(x => x.PreferredLanguage);
  }

  [Theory]
  [InlineData("En")]
  [InlineData("Es")]
  [InlineData("Fr")]
  [InlineData("De")]
  public void Validator_HasNoErrors_WhenPreferredLanguageIsValid(string language)
  {
    var command = new ChangePreferredLanguageCommand
    {
      UserId = Guid.NewGuid(),
      PreferredLanguage = language
    };

    var result = _validator.TestValidate(command);

    result.ShouldNotHaveAnyValidationErrors();
  }

  [Fact]
  public void Validator_HasNoErrors_WhenCommandIsValid()
  {
    var command = new ChangePreferredLanguageCommand
    {
      UserId = Guid.NewGuid(),
      PreferredLanguage = "En"
    };

    var result = _validator.TestValidate(command);

    result.ShouldNotHaveAnyValidationErrors();
  }
}