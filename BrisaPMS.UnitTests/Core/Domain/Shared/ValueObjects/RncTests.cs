using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Shared.ValueObjects;

public class RncTests
{
    [Fact]
    public void Constructor_ShouldCreateRnc_WhenBusinessRncIsValid()
    {
        // Arrange
        const string rawRnc = "123456789";

        // Act
        var result = new Rnc(rawRnc);

        // Assert
        result.Value.Should().Be(rawRnc);
    }

    [Fact]
    public void Constructor_ShouldCreateRnc_WhenPersonRncIsValid()
    {
        // Arrange
        const string rawRnc = "00112345678";

        // Act
        var result = new Rnc(rawRnc);

        // Assert
        result.Value.Should().Be(rawRnc);
    }

    [Fact]
    public void Constructor_ShouldCreateRnc_WhenRncContainsFormattingCharacters()
    {
        // Arrange
        const string rawRnc = " 001-1234567-8 ";

        // Act
        var result = new Rnc(rawRnc);

        // Assert
        result.Value.Should().Be("00112345678");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenRncIsNullOrWhiteSpace(string? rawRnc)
    {
        // Arrange + Act
        Action act = () => _ = new Rnc(rawRnc!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Theory]
    [InlineData("12345678")]
    [InlineData("1234567890")]
    [InlineData("123456789012")]
    public void Constructor_ShouldThrowBusinessRuleException_WhenRncLengthIsNot9Or11Digits(string rawRnc)
    {
        // Arrange + Act
        Action act = () => _ = new Rnc(rawRnc);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }
}
