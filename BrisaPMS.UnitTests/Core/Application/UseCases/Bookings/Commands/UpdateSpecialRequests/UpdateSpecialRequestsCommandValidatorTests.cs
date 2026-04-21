using BrisaPMS.Application.UseCases.Bookings.Commands.UpdateSpecialRequests;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Commands.UpdateSpecialRequests;

public class UpdateSpecialRequestsCommandValidatorTests
{
    private readonly UpdateSpecialRequestsCommandValidator _validator;

    public UpdateSpecialRequestsCommandValidatorTests()
    {
        _validator = new UpdateSpecialRequestsCommandValidator();
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
        result.ShouldHaveValidationErrorFor(x => x.SpecialRequests);
    }

    [Fact]
    public void Validator_HasErrors_WhenSpecialRequestsExceedsLimit()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), new string('A', 501));

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SpecialRequests);
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

    private static UpdateSpecialRequestsCommand CreateValidCommand()
    {
        return CreateCommand(Guid.NewGuid(), "High floor please");
    }

    private static UpdateSpecialRequestsCommand CreateCommand(Guid bookingId, string specialRequests)
    {
        return new UpdateSpecialRequestsCommand
        {
            BookingId = bookingId,
            SpecialRequests = specialRequests
        };
    }
}