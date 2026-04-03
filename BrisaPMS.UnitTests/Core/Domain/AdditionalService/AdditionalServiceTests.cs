using BrisaPMS.Domain.AdditionalService;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.AdditionalService;

public class AdditionalServiceTests
{
    [Fact]
    public void Constructor_ShouldCreateAdditionalService_WhenValuesAreValid()
    {
        // Arrange
        const string name = "Airport Pickup";
        const string description = "Transportation from the airport to the hotel";

        // Act
        var result = new AdditionalService(name, description, 45m, CurrencyCode.USD);

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
        result.Name.Should().Be(name);
        result.Description.Should().Be(description);
        result.Price.Should().Be(45m);
        result.CurrencyCode.Should().Be(CurrencyCode.USD);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenNameIsNullOrWhiteSpace(string? name)
    {
        // Arrange
        const string description = "Transportation from the airport to the hotel";

        // Act
        Action act = () => _ = new AdditionalService(name!, description, 45m, CurrencyCode.USD);

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
        const string name = "Airport Pickup";

        // Act
        Action act = () => _ = new AdditionalService(name, description!, 45m, CurrencyCode.USD);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenPriceIsNegative()
    {
        // Arrange
        const decimal price = -1m;

        // Act
        Action act = () => _ = new AdditionalService("Airport Pickup", "Transportation from the airport to the hotel", price, CurrencyCode.USD);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenCurrencyCodeIsInvalid()
    {
        // Arrange
        var invalidCurrencyCode = (CurrencyCode)999;

        // Act
        Action act = () => _ = new AdditionalService("Airport Pickup", "Transportation from the airport to the hotel", 45m, invalidCurrencyCode);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateName_ShouldUpdateName_WhenValueIsValid()
    {
        // Arrange
        var additionalService = CreateAdditionalService();

        // Act
        additionalService.UpdateName("Late Checkout");

        // Assert
        additionalService.Name.Should().Be("Late Checkout");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void UpdateName_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrWhiteSpace(string? newName)
    {
        // Arrange
        var additionalService = CreateAdditionalService();

        // Act
        Action act = () => additionalService.UpdateName(newName!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void UpdateDescription_ShouldUpdateDescription_WhenValueIsValid()
    {
        // Arrange
        var additionalService = CreateAdditionalService();

        // Act
        additionalService.UpdateDescription("Room service breakfast");

        // Assert
        additionalService.Description.Should().Be("Room service breakfast");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void UpdateDescription_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrWhiteSpace(string? newDescription)
    {
        // Arrange
        var additionalService = CreateAdditionalService();

        // Act
        Action act = () => additionalService.UpdateDescription(newDescription!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void UpdatePrice_ShouldUpdatePrice_WhenValueIsValid()
    {
        // Arrange
        var additionalService = CreateAdditionalService();

        // Act
        additionalService.UpdatePrice(60m);

        // Assert
        additionalService.Price.Should().Be(60m);
    }

    [Fact]
    public void UpdatePrice_ShouldThrowBusinessRuleException_WhenValueIsNegative()
    {
        // Arrange
        var additionalService = CreateAdditionalService();

        // Act
        Action act = () => additionalService.UpdatePrice(-1m);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateCurrencyCode_ShouldUpdateCurrencyCode_WhenValueIsValid()
    {
        // Arrange
        var additionalService = CreateAdditionalService();

        // Act
        additionalService.UpdateCurrencyCode(CurrencyCode.DOP);

        // Assert
        additionalService.CurrencyCode.Should().Be(CurrencyCode.DOP);
    }

    [Fact]
    public void UpdateCurrencyCode_ShouldThrowBusinessRuleException_WhenValueIsInvalid()
    {
        // Arrange
        var additionalService = CreateAdditionalService();
        var invalidCurrencyCode = (CurrencyCode)999;

        // Act
        Action act = () => additionalService.UpdateCurrencyCode(invalidCurrencyCode);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    private static AdditionalService CreateAdditionalService()
    {
        return new AdditionalService(
            "Airport Pickup",
            "Transportation from the airport to the hotel",
            45m,
            CurrencyCode.USD);
    }
}
