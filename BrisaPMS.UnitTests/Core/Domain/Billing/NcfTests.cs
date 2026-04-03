using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Shared.Exceptions;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Billing;

public class NcfTests
{
    [Fact]
    public void Constructor_ShouldCreateNcf_WhenValueIsValid()
    {
        // Arrange
        const string value = "B01-00000000001";

        // Act
        var result = new Ncf(value);

        // Assert
        result.Value.Should().Be(value);
        result.Series.Should().Be("B01");
        result.Sequence.Should().Be("B01-");
        result.Type.Should().Be(InvoiceType.FacturaCredito);
    }

    [Fact]
    public void Constructor_ShouldNormalizeValueToUpperCase_WhenValueIsLowerCase()
    {
        // Arrange
        const string value = "b02-00000000001";

        // Act
        var result = new Ncf(value);

        // Assert
        result.Value.Should().Be("B02-00000000001");
        result.Type.Should().Be(InvoiceType.FacturaConsumo);
    }

    [Theory]
    [InlineData("B04-00000000001", InvoiceType.NotaDeCredito)]
    [InlineData("B15-00000000001", InvoiceType.NotaDeDebito)]
    [InlineData("B14-00000000001", InvoiceType.RegimeEspecial)]
    public void Type_ShouldReturnExpectedInvoiceType_WhenSeriesIsSupported(string value, InvoiceType expectedType)
    {
        // Arrange
        var ncf = new Ncf(value);

        // Act
        var result = ncf.Type;

        // Assert
        result.Should().Be(expectedType);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_ShouldThrowBusinessRuleException_WhenValueIsNullOrWhiteSpace(string? value)
    {
        // Act
        Action act = () => _ = new Ncf(value!);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Theory]
    [InlineData("A01-00000000001")]
    [InlineData("B00-00000000001")]
    [InlineData("B01-0000000001")]
    [InlineData("B01-000000000001")]
    [InlineData("B0100000000001")]
    [InlineData("B01-ABCDEFGHIJK")]
    public void Constructor_ShouldThrowBusinessRuleException_WhenFormatIsInvalid(string value)
    {
        // Act
        Action act = () => _ = new Ncf(value);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }
}
