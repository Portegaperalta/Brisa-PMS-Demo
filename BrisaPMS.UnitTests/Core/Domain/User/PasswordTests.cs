using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.User;

public class PasswordTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenPasswordIsNullOrWhiteSpace(string? password)
    {
        // Arrange + Act
        Action act = () => _ = new Password(password!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowInvalidFieldException_WhenPasswordIsShorterThan8Characters()
    {
        // Arrange
        const string password = "Pass1!";

        // Act
        Action act = () => _ = new Password(password);

        // Assert
        act.Should().Throw<InvalidFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowInvalidFieldException_WhenPasswordDoesNotContainSpecialCharacter()
    {
        // Arrange
        const string password = "Password123";

        // Act
        Action act = () => _ = new Password(password);

        // Assert
        act.Should().Throw<InvalidFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowInvalidFieldException_WhenPasswordDoesNotContainUppercaseLetter()
    {
        // Arrange
        const string password = "password123!";

        // Act
        Action act = () => _ = new Password(password);

        // Assert
        act.Should().Throw<InvalidFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowInvalidFieldException_WhenPasswordExceeds512Characters()
    {
        // Arrange
        var password = $"{new string('a', 511)}A!";

        // Act
        Action act = () => _ = new Password(password);

        // Assert
        act.Should().Throw<InvalidFieldException>();
    }
}
