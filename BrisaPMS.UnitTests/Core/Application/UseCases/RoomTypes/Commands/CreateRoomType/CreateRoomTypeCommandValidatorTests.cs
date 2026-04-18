using BrisaPMS.Application.UseCases.RoomTypes.Commands.CreateRoomType;
using FluentValidation.TestHelper;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.RoomTypes.Commands.CreateRoomType;

public class CreateRoomTypeCommandValidatorTests
{
    private readonly CreateRoomTypeCommandValidator _validator;

    public CreateRoomTypeCommandValidatorTests()
    {
        _validator = new CreateRoomTypeCommandValidator();
    }

    [Fact]
    public void Validator_HasErrors_WhenRequiredFieldsAreEmpty()
    {
        // Arrange
        var command = CreateCommand(string.Empty, null, default, default, string.Empty, default, default);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.BaseRate);
        result.ShouldHaveValidationErrorFor(x => x.TotalBeds);
        result.ShouldHaveValidationErrorFor(x => x.BedType);
        result.ShouldHaveValidationErrorFor(x => x.MaxOccupancyAdults);
        result.ShouldHaveValidationErrorFor(x => x.MaxOccupancyChildren);
    }

    [Fact]
    public void Validator_HasErrors_WhenNameOrDescriptionExceedMaxLength()
    {
        // Arrange
        var command = CreateCommand
        (
            new string('R', 101),
            new string('D', 501),
            25m,
            2,
            "Double",
            2,
            1
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Theory]
    [InlineData(-1, 2, "Double", 2, 1)]
    [InlineData(101, 2, "Double", 2, 1)]
    [InlineData(25, 0, "Double", 2, 1)]
    [InlineData(25, 21, "Double", 2, 1)]
    [InlineData(25, 2, "Invalid", 2, 1)]
    [InlineData(25, 2, "Double", 0, 1)]
    [InlineData(25, 2, "Double", 17, 1)]
    [InlineData(25, 2, "Double", 2, 11)]
    public void Validator_HasErrors_WhenCommandValuesAreOutOfRange(
        decimal baseRate,
        int totalBeds,
        string bedType,
        int maxOccupancyAdults,
        int maxOccupancyChildren)
    {
        // Arrange
        var command = CreateCommand
        (
            "Deluxe Suite",
            "Ocean view",
            baseRate,
            totalBeds,
            bedType,
            maxOccupancyAdults,
            maxOccupancyChildren
        );

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        if (baseRate is < 0 or > 100)
            result.ShouldHaveValidationErrorFor(x => x.BaseRate);

        if (totalBeds is <= 0 or > 20)
            result.ShouldHaveValidationErrorFor(x => x.TotalBeds);

        if (bedType == "Invalid")
            result.ShouldHaveValidationErrorFor(x => x.BedType);

        if (maxOccupancyAdults is <= 0 or > 16)
            result.ShouldHaveValidationErrorFor(x => x.MaxOccupancyAdults);

        if (maxOccupancyChildren is < 0 or > 10)
            result.ShouldHaveValidationErrorFor(x => x.MaxOccupancyChildren);
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

    private static CreateRoomTypeCommand CreateValidCommand()
    {
        return CreateCommand("Deluxe Suite", "Ocean view room", 25m, 2, "Double", 2, 1);
    }

    private static CreateRoomTypeCommand CreateCommand(
        string name,
        string? description,
        decimal baseRate,
        int totalBeds,
        string bedType,
        int maxOccupancyAdults,
        int maxOccupancyChildren)
    {
        return new CreateRoomTypeCommand
        {
            Name = name,
            Description = description,
            BaseRate = baseRate,
            TotalBeds = totalBeds,
            BedType = bedType,
            MaxOccupancyAdults = maxOccupancyAdults,
            MaxOccupancyChildren = maxOccupancyChildren
        };
    }
}
