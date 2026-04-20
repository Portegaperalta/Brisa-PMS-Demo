using BrisaPMS.Application.UseCases.Bookings.Commands.UpdateGuestCount;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Bookings.Commands.UpdateGuestCount;

public class UpdateGuestCountCommandValidatorTests
{
    private readonly UpdateGuestCountCommandValidator _validator;

    public UpdateGuestCountCommandValidatorTests()
    {
        _validator = new UpdateGuestCountCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenBookingIdIsEmpty()
    {
        // Arrange
        var command = CreateCommand(Guid.Empty, 2, 1);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BookingId);
    }

    [Fact]
    public void Validator_HasErrors_WhenNumberOfAdultsIsEmpty()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), default, 1);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NumberOfAdults);
    }

    [Fact]
    public void Validator_HasErrors_WhenNumberOfAdultsIsZero()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), 0, 1);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NumberOfAdults);
    }

    [Fact]
    public void Validator_HasErrors_WhenNumberOfAdultsExceedsLimit()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), 11, 1);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NumberOfAdults);
    }

    [Fact]
    public void Validator_HasErrors_WhenNumberOfChildrenIsNegative()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), 2, -1);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NumberOfChildren);
    }

    [Fact]
    public void Validator_HasErrors_WhenNumberOfChildrenExceedsLimit()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), 2, 11);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.NumberOfChildren);
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateCommand(Guid.Empty, default, default);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.BookingId);
        result.ShouldHaveValidationErrorFor(x => x.NumberOfAdults);
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
    [InlineData(1, 0)]
    [InlineData(2, 1)]
    [InlineData(10, 10)]
    public void Validator_HasNoErrors_WhenGuestCountsAreValid(int numberOfAdults, int numberOfChildren)
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid(), numberOfAdults, numberOfChildren);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    private static UpdateGuestCountCommand CreateValidCommand()
    {
        return CreateCommand(Guid.NewGuid(), 2, 1);
    }

    private static UpdateGuestCountCommand CreateCommand(Guid bookingId, int numberOfAdults, int numberOfChildren)
    {
        return new UpdateGuestCountCommand
        {
            BookingId = bookingId,
            NumberOfAdults = numberOfAdults,
            NumberOfChildren = numberOfChildren
        };
    }
}