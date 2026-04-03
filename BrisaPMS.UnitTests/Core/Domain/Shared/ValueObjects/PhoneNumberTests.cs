using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Shared.ValueObjects;

public class PhoneNumberTests
{
    [Fact]
    public void Constructor_ShouldCreatePhoneNumber_WhenPhoneNumberIsValid()
    {
        // Arrange
        const string phoneNumber = "+1 809 555 1234";

        // Act
        var result = new PhoneNumber(phoneNumber);

        // Assert
        result.Value.Should().Be(phoneNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenPhoneNumberIsNullOrWhiteSpace(string? phoneNumber)
    {
        // Arrange + Act
        Action act = () => _ = new PhoneNumber(phoneNumber!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldCreatePhoneNumber_WhenPhoneNumberLengthIs25Characters()
    {
        // Arrange
        var phoneNumber = new string('1', 25);

        // Act
        var result = new PhoneNumber(phoneNumber);

        // Assert
        result.Value.Should().Be(phoneNumber);
    }

    [Fact]
    public void Constructor_ShouldThrowMaxCharacterLimitException_WhenPhoneNumberExceeds25Characters()
    {
        // Arrange
        var phoneNumber = new string('1', 26);

        // Act
        Action act = () => _ = new PhoneNumber(phoneNumber);

        // Assert
        act.Should().Throw<MaxCharacterLimitException>();
    }
}
