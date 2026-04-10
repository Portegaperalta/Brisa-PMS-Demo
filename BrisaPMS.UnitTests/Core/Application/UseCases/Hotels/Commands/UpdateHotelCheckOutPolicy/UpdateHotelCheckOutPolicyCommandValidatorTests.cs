using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelCheckOutPolicy;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Hotels.Commands.UpdateHotelCheckOutPolicy;

public class UpdateHotelCheckOutPolicyCommandValidatorTests
{
  private readonly UpdateHotelCheckOutPolicyCommandValidator _validator;

  public UpdateHotelCheckOutPolicyCommandValidatorTests()
  {
    _validator = new UpdateHotelCheckOutPolicyCommandValidator();
  }

  [Fact]
  public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
  {
    // Arrange
    var command = CreateUpdateHotelCheckOutPolicyCommand(Guid.Empty, default, default);

    // Act
    var result = _validator.TestValidate(command);

    // Assert
    result.ShouldHaveValidationErrorFor(x => x.HotelId);
    result.ShouldHaveValidationErrorFor(x => x.CheckInTime);
    result.ShouldHaveValidationErrorFor(x => x.CheckOutTime);
  }

  // Helper methods
  private static UpdateHotelCheckOutPolicyCommand CreateUpdateHotelCheckOutPolicyCommand(
      Guid hotelId,
      TimeOnly checkInTime,
      TimeOnly checkOutTime)
  {
    return new UpdateHotelCheckOutPolicyCommand
    {
      HotelId = hotelId,
      CheckInTime = checkInTime,
      CheckOutTime = checkOutTime
    };
  }
}
