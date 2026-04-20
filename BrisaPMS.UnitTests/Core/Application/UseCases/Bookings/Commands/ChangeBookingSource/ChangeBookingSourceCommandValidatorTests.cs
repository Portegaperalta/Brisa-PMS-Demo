using BrisaPMS.Application.UseCases.Bookings.Commands.ChangeBookingSource;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Commands.ChangeBookingSource;

public class ChangeBookingSourceCommandValidatorTests
{
  private readonly ChangeBookingSourceCommandValidator _validator;

  public ChangeBookingSourceCommandValidatorTests()
  {
    _validator = new ChangeBookingSourceCommandValidator();
  }


  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = CreateCommand(Guid.Empty, string.Empty);

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.BookingId);
    result.ShouldHaveValidationErrorFor(x => x.Source);
  }

  [Fact]
  public void Validator_HasErrors_WhenSourceExceedsLimit()
  {
    // Arrange
    var command = CreateCommand(Guid.NewGuid(), new string('A', 201));

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Source);
  }

  [Fact]
  public void Validator_HasErrors_WhenSourceIsInvalid()
  {
    // Arrange
    var command = CreateCommand(Guid.NewGuid(), "InvalidSource");

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Source);
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

  [Theory]
  [InlineData("Website")]
  [InlineData("InPerson")]
  [InlineData("Phone")]
  [InlineData("ThirdParty")]
  public void Validator_HasNoErrors_WhenSourceIsValid(string source)
  {
    // Arrange
    var command = CreateCommand(Guid.NewGuid(), source);

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }

  private static ChangeBookingSourceCommand CreateValidCommand()
  {
    return CreateCommand(Guid.NewGuid(), "Website");
  }

  private static ChangeBookingSourceCommand CreateCommand(Guid bookingId, string source)
  {
    return new ChangeBookingSourceCommand
    {
      BookingId = bookingId,
      Source = source
    };
  }
}