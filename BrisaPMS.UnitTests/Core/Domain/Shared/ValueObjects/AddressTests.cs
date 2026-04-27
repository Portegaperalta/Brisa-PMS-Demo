using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Shared.ValueObjects;

public class AddressTests
{
    [Fact]
    public void Constructor_ShouldCreateAddress_WhenValuesAreValid()
    {
        // Arrange
        var address1 = "123 Main Street";
        var address2 = "Apartment 4B";
        var city = "Santo Domingo";
        var province = "Distrito Nacional";
        var zipCode = "10101";

        // Act
        var result = new Address(address1, address2, city, province, zipCode);

        // Assert
        result.Address1.Should().Be(address1);
        result.Address2.Should().Be(address2);
        result.City.Should().Be(city);
        result.Province.Should().Be(province);
        result.ZipCode.Should().Be(zipCode);
    }

    [Fact]
    public void Constructor_ShouldCreateAddress_WhenAddress2IsNull()
    {
        // Arrange
        var address1 = "123 Main Street";
        var city = "Santo Domingo";
        var province = "Distrito Nacional";
        var zipCode = "10101";

        // Act
        Action act = () => _ = new Address(address1, null, city, province, zipCode);

        // Assert
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData("Address 1")]
    [InlineData("City")]
    [InlineData("Province")]
    [InlineData("Zip Code")]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenRequiredFieldIsNullOrWhiteSpace(string fieldName)
    {
        // Arrange
        var address1 = "123 Main Street";
        var address2 = "Apartment 4B";
        var city = "Santo Domingo";
        var province = "Distrito Nacional";
        var zipCode = "10101";

        switch (fieldName)
        {
            case "Address 1":
                address1 = " ";
                break;
            case "City":
                city = " ";
                break;
            case "Province":
                province = " ";
                break;
            case "Zip Code":
                zipCode = " ";
                break;
        }

        // Act
        Action act = () => _ = new Address(address1, address2, city, province, zipCode);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Theory]
    [InlineData("10A01")]
    [InlineData("10-01")]
    public void Constructor_ShouldThrowBusinessRuleException_WhenZipCodeContainsNonNumericCharacters(string zipCode)
    {
        // Arrange
        var address1 = "123 Main Street";
        var address2 = "Apartment 4B";
        var city = "Santo Domingo";
        var province = "Distrito Nacional";

        // Act
        Action act = () => _ = new Address(address1, address2, city, province, zipCode);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }
}
