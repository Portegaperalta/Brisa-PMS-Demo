using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Shared.ValueObjects;

public class UrlTests
{
    [Fact]
    public void Constructor_ShouldCreateUrl_WhenUrlIsValid()
    {
        // Arrange
        const string rawUrl = "https://example.com/path";

        // Act
        var result = new Url(rawUrl);

        // Assert
        result.Value.Should().Be(rawUrl);
    }

    [Fact]
    public void Constructor_ShouldCreateUrl_WhenUrlIsValidHttp()
    {
        // Arrange
        const string rawUrl = "http://example.com";

        // Act
        var result = new Url(rawUrl);

        // Assert
        result.Value.Should().Be(rawUrl);
    }

    [Fact]
    public void Constructor_ShouldCreateUrl_WhenUrlContainsLeadingAndTrailingSpaces()
    {
        // Arrange
        const string rawUrl = "  https://example.com/path  ";

        // Act
        var result = new Url(rawUrl);

        // Assert
        result.Value.Should().Be("https://example.com/path");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenUrlIsNullOrWhiteSpace(string? rawUrl)
    {
        // Arrange + Act
        Action act = () => _ = new Url(rawUrl!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Theory]
    [InlineData("example")]
    [InlineData("www.example.com")]
    [InlineData("https:/example.com")]
    public void Constructor_ShouldThrowInvalidFieldException_WhenUrlFormatIsInvalid(string rawUrl)
    {
        // Arrange + Act
        Action act = () => _ = new Url(rawUrl);

        // Assert
        act.Should().Throw<InvalidFieldException>();
    }

    [Theory]
    [InlineData("ftp://example.com")]
    [InlineData("file://example.com")]
    public void Constructor_ShouldThrowInvalidFieldException_WhenUrlSchemeIsNotHttpOrHttps(string rawUrl)
    {
        // Arrange + Act
        Action act = () => _ = new Url(rawUrl);

        // Assert
        act.Should().Throw<InvalidFieldException>();
    }
}
