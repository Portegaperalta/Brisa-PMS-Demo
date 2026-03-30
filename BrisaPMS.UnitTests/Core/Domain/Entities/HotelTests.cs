using BrisaPMS.Domain.Entities;
using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Entities;

public class HotelTests
{
    [Fact]
    public void Constructor_ShouldCreateHotel_WhenValuesAreValid()
    {
        // Arrange
        var logoUrl = CreateUrl();
        var businessEmail = CreateEmail();
        var businessPhoneNumber = CreatePhoneNumber();
        var address = CreateAddress();
        var checkInOutTimes = CreateCheckInOutTimes();

        // Act
        var result = new Hotel(
            "Brisa Hospitality SRL",
            "Hotel Brisa",
            logoUrl,
            businessEmail,
            businessPhoneNumber,
            address,
            checkInOutTimes,
            CurrencyCode.DOP,
            18m,
            10m,
            true);

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
        result.LegalName.Should().Be("Brisa Hospitality SRL");
        result.CommercialName.Should().Be("Hotel Brisa");
        result.LogoUrl.Should().Be(logoUrl);
        result.BusinessEmail.Should().Be(businessEmail);
        result.BusinessPhoneNumber.Should().Be(businessPhoneNumber);
        result.Address.Should().Be(address);
        result.CheckInOutTimes.Should().Be(checkInOutTimes);
        result.DefaultCurrencyCode.Should().Be(CurrencyCode.DOP);
        result.ItbisRate.Should().Be(18m);
        result.ServiceChargeRate.Should().Be(10m);
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenLegalNameIsNull()
    {
        // Arrange
        string legalName = null!;

        // Act
        Action act = () => _ = new Hotel(
            legalName,
            "Hotel Brisa",
            CreateUrl(),
            CreateEmail(),
            CreatePhoneNumber(),
            CreateAddress(),
            CreateCheckInOutTimes(),
            CurrencyCode.DOP,
            18m,
            10m,
            true);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenLegalNameIsWhiteSpace()
    {
        // Arrange
        const string legalName = " ";

        // Act
        Action act = () => _ = new Hotel(
            legalName,
            "Hotel Brisa",
            CreateUrl(),
            CreateEmail(),
            CreatePhoneNumber(),
            CreateAddress(),
            CreateCheckInOutTimes(),
            CurrencyCode.DOP,
            18m,
            10m,
            true);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenCommercialNameIsNull()
    {
        // Arrange
        string commercialName = null!;

        // Act
        Action act = () => _ = new Hotel(
            "Brisa Hospitality SRL",
            commercialName,
            CreateUrl(),
            CreateEmail(),
            CreatePhoneNumber(),
            CreateAddress(),
            CreateCheckInOutTimes(),
            CurrencyCode.DOP,
            18m,
            10m,
            true);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenCommercialNameIsWhiteSpace()
    {
        // Arrange
        const string commercialName = " ";

        // Act
        Action act = () => _ = new Hotel(
            "Brisa Hospitality SRL",
            commercialName,
            CreateUrl(),
            CreateEmail(),
            CreatePhoneNumber(),
            CreateAddress(),
            CreateCheckInOutTimes(),
            CurrencyCode.DOP,
            18m,
            10m,
            true);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowCurrencyNotSupportedException_WhenCurrencyIsInvalid()
    {
        // Arrange
        var invalidCurrency = (CurrencyCode)999;

        // Act
        Action act = () => _ = new Hotel(
            "Brisa Hospitality SRL",
            "Hotel Brisa",
            CreateUrl(),
            CreateEmail(),
            CreatePhoneNumber(),
            CreateAddress(),
            CreateCheckInOutTimes(),
            invalidCurrency,
            18m,
            10m,
            true);

        // Assert
        act.Should().Throw<CurrencyNotSupportedException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenItbisRateIsNegative()
    {
        // Arrange + Act
        Action act = () => _ = new Hotel(
            "Brisa Hospitality SRL",
            "Hotel Brisa",
            CreateUrl(),
            CreateEmail(),
            CreatePhoneNumber(),
            CreateAddress(),
            CreateCheckInOutTimes(),
            CurrencyCode.DOP,
            -1m,
            10m,
            true);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenServiceChargeRateIsNegative()
    {
        // Arrange + Act
        Action act = () => _ = new Hotel(
            "Brisa Hospitality SRL",
            "Hotel Brisa",
            CreateUrl(),
            CreateEmail(),
            CreatePhoneNumber(),
            CreateAddress(),
            CreateCheckInOutTimes(),
            CurrencyCode.DOP,
            18m,
            -1m,
            true);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void ChangeLegalName_ShouldUpdateLegalName_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();

        // Act
        hotel.ChangeLegalName("Brisa Resorts SRL");

        // Assert
        hotel.LegalName.Should().Be("Brisa Resorts SRL");
    }

    [Fact]
    public void ChangeLegalName_ShouldThrowEmptyRequiredFieldException_WhenValueIsWhiteSpace()
    {
        // Arrange
        var hotel = CreateHotel();

        // Act
        Action act = () => hotel.ChangeLegalName(" ");

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void ChangeCommercialName_ShouldUpdateCommercialName_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();

        // Act
        hotel.ChangeCommercialName("Brisa Beach");

        // Assert
        hotel.CommercialName.Should().Be("Brisa Beach");
    }

    [Fact]
    public void ChangeCommercialName_ShouldThrowEmptyRequiredFieldException_WhenValueIsWhiteSpace()
    {
        // Arrange
        var hotel = CreateHotel();

        // Act
        Action act = () => hotel.ChangeCommercialName(" ");

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void ChangeLogoUrl_ShouldUpdateLogoUrl_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();
        var newLogoUrl = new Url("https://cdn.example.com/hotel-logo.png");

        // Act
        hotel.ChangeLogoUrl(newLogoUrl);

        // Assert
        hotel.LogoUrl.Should().Be(newLogoUrl);
    }

    [Fact]
    public void ChangeBusinessEmail_ShouldUpdateBusinessEmail_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();
        var newBusinessEmail = new Email("frontdesk@hotelbrisa.com");

        // Act
        hotel.ChangeBusinessEmail(newBusinessEmail);

        // Assert
        hotel.BusinessEmail.Should().Be(newBusinessEmail);
    }

    [Fact]
    public void ChangeBusinessPhoneNumber_ShouldUpdateBusinessPhoneNumber_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();
        var newBusinessPhoneNumber = new PhoneNumber("+1 829 555 4321");

        // Act
        hotel.ChangeBusinessPhoneNumber(newBusinessPhoneNumber);

        // Assert
        hotel.BusinessPhoneNumber.Should().Be(newBusinessPhoneNumber);
    }

    [Fact]
    public void ChangeAddress_ShouldUpdateAddress_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();
        var newAddress = new Address("456 Ocean Drive", "Suite 8", "Punta Cana", "La Altagracia", "23000");

        // Act
        hotel.ChangeAddress(newAddress);

        // Assert
        hotel.Address.Should().Be(newAddress);
    }

    [Fact]
    public void ChangeCheckInOutTimes_ShouldUpdateCheckInOutTimes_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();
        var newCheckInOutTimes = new CheckInOutTimes(
            new DateTime(2026, 3, 28, 15, 0, 0),
            new DateTime(2026, 3, 29, 11, 0, 0));

        // Act
        hotel.ChangeCheckInOutTimes(newCheckInOutTimes);

        // Assert
        hotel.CheckInOutTimes.Should().Be(newCheckInOutTimes);
    }

    [Fact]
    public void ChangeDefaultCurrencyCode_ShouldUpdateDefaultCurrencyCode_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();

        // Act
        hotel.ChangeDefaultCurrencyCode(CurrencyCode.USD);

        // Assert
        hotel.DefaultCurrencyCode.Should().Be(CurrencyCode.USD);
    }

    [Fact]
    public void ChangeDefaultCurrencyCode_ShouldThrowCurrencyNotSupportedException_WhenValueIsInvalid()
    {
        // Arrange
        var hotel = CreateHotel();
        var invalidCurrency = (CurrencyCode)999;

        // Act
        Action act = () => hotel.ChangeDefaultCurrencyCode(invalidCurrency);

        // Assert
        act.Should().Throw<CurrencyNotSupportedException>();
    }

    [Fact]
    public void ChangeItbisRate_ShouldUpdateItbisRate_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();

        // Act
        hotel.ChangeItbisRate(16m);

        // Assert
        hotel.ItbisRate.Should().Be(16m);
    }

    [Fact]
    public void ChangeItbisRate_ShouldThrowBusinessRuleException_WhenValueIsNegative()
    {
        // Arrange
        var hotel = CreateHotel();

        // Act
        Action act = () => hotel.ChangeItbisRate(-1m);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void ChangeServiceChargeRate_ShouldUpdateServiceChargeRate_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();

        // Act
        hotel.ChangeServiceChargeRate(12m);

        // Assert
        hotel.ServiceChargeRate.Should().Be(12m);
    }

    [Fact]
    public void ChangeServiceChargeRate_ShouldThrowBusinessRuleException_WhenValueIsNegative()
    {
        // Arrange
        var hotel = CreateHotel();

        // Act
        Action act = () => hotel.ChangeServiceChargeRate(-1m);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    private static Hotel CreateHotel()
    {
        return new Hotel(
            "Brisa Hospitality SRL",
            "Hotel Brisa",
            CreateUrl(),
            CreateEmail(),
            CreatePhoneNumber(),
            CreateAddress(),
            CreateCheckInOutTimes(),
            CurrencyCode.DOP,
            18m,
            10m,
            true);
    }

    private static Url CreateUrl()
    {
        return new Url("https://example.com/logo.png");
    }

    private static Email CreateEmail()
    {
        return new Email("contact@hotelbrisa.com");
    }

    private static PhoneNumber CreatePhoneNumber()
    {
        return new PhoneNumber("+1 809 555 1234");
    }

    private static Address CreateAddress()
    {
        return new Address("123 Main Street", "Suite 4B", "Santo Domingo", "Distrito Nacional", "10101");
    }

    private static CheckInOutTimes CreateCheckInOutTimes()
    {
        return new CheckInOutTimes(
            new DateTime(2026, 3, 28, 14, 0, 0),
            new DateTime(2026, 3, 29, 12, 0, 0));
    }
}
