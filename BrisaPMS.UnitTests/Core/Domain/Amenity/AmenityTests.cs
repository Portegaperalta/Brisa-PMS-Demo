
using BrisaPMS.Domain.Shared.Exceptions;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Amenity;

public class AmenityTests
{
    [Fact]
    public void Constructor_ShouldCreateAmenity_WhenValuesAreValid()
    {
        // Arrange
        const string name = "Pool Access";
        const string description = "Access to the swimming pool";

        // Act
        var result = new Amen(name, description);

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
        result.Name.Should().Be(name);
        result.Description.Should().Be(description);
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Constructor_ShouldCreateAmenity_WhenIsActiveIsFalse()
    {
        // Arrange
        const string name = "Spa Access";
        const string description = "Access to the spa area";

        // Act
        var result = new Amenity(name, description, false);

        // Assert
        result.IsActive.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenNameIsNullOrWhiteSpace(string? name)
    {
        // Arrange
        const string description = "Access to the swimming pool";

        // Act
        Action act = () => _ = new Amenity(name!, description);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenDescriptionIsNullOrWhiteSpace(string? description)
    {
        // Arrange
        const string name = "Pool Access";

        // Act
        Action act = () => _ = new Amenity(name, description!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void UpdateName_ShouldUpdateName_WhenValueIsValid()
    {
        // Arrange
        var amenity = CreateAmenity();

        // Act
        amenity.UpdateName("Gym Access");

        // Assert
        amenity.Name.Should().Be("Gym Access");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void UpdateName_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrWhiteSpace(string? newName)
    {
        // Arrange
        var amenity = CreateAmenity();

        // Act
        Action act = () => amenity.UpdateName(newName!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void UpdateDescription_ShouldUpdateDescription_WhenValueIsValid()
    {
        // Arrange
        var amenity = CreateAmenity();

        // Act
        amenity.UpdateDescription("Access to the modern gym area");

        // Assert
        amenity.Description.Should().Be("Access to the modern gym area");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void UpdateDescription_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrWhiteSpace(string? newDescription)
    {
        // Arrange
        var amenity = CreateAmenity();

        // Act
        Action act = () => amenity.UpdateDescription(newDescription!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void SetAsInactive_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var amenity = CreateAmenity();

        // Act
        amenity.SetAsInactive();

        // Assert
        amenity.IsActive.Should().BeFalse();
    }

    [Fact]
    public void SetAsActive_ShouldSetIsActiveToTrue()
    {
        // Arrange
        var amenity = CreateAmenity(false);

        // Act
        amenity.SetAsActive();

        // Assert
        amenity.IsActive.Should().BeTrue();
    }

    private static Amenity CreateAmenity(bool isActive = true)
    {
        return new Amenity("Pool Access", "Access to the swimming pool", isActive);
    }
}
