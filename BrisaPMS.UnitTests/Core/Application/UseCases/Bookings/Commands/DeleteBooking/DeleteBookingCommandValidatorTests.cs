using BrisaPMS.Application.UseCases.Bookings.Commands.DeleteBooking;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Commands.DeleteBooking;

public class DeleteBookingCommandValidatorTests
{
  private readonly DeleteBookingCommandValidator _validator;

  public DeleteBookingCommandValidatorTests()
  {
    _validator = new DeleteBookingCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = new DeleteBookingCommand { Id = Guid.Empty };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.Id);
  }

  [Fact]
  public void Validator_HasNoErrors_WhenCommandIsValid()
  {
    // Arrange
    var command = new DeleteBookingCommand { Id = Guid.NewGuid() };

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldNotHaveAnyValidationErrors();
  }
}