using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Shared.ValueObjects;

public class EmailTests
{
    [Fact]
    public void Constructor_ShouldCreateEmail_WhenEmailIsValid()
    {
        // Arrange
        const string emailAddress = "john.doe@example.com";

        // Act
        var result = new Email(emailAddress);

        // Assert
        result.Value.Should().Be(emailAddress);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenEmailIsNullOrWhiteSpace_(string? emailAddress)
    {
        // Arrange + Act
        Action act = () => _ = new Email(emailAddress!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Theory]
    [InlineData("plainaddress")]
    [InlineData("missing-at-sign.com")]
    [InlineData("missingdot@example")]
    public void Constructor_ShouldThrowInvalidFieldException_WhenEmailDoesNotContainRequiredEmailParts(string emailAddress)
    {
        // Arrange + Act
        Action act = () => _ = new Email(emailAddress);

        // Assert
        act.Should().Throw<InvalidFieldException>();
    }

    [Fact]
    public void Constructor_ShouldCreateEmail_WhenEmailLengthIs254Characters()
    {
        // Arrange
        var localPart = new string('a', 242);
        var emailAddress = $"{localPart}@b.com";

        // Act
        var result = new Email(emailAddress);

        // Assert
        result.Value.Should().Be(emailAddress);
    }

    [Fact]
    public void Constructor_ShouldThrowMaxCharacterLimitException_WhenEmailExceeds254Characters()
    {
        // Arrange
        var localPart = new string('a', 255);
        var emailAddress = $"{localPart}@b.com";

        // Act
        Action act = () => _ = new Email(emailAddress);

        // Assert
        act.Should().Throw<MaxCharacterLimitException>();
    }
}
