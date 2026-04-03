using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Hotels;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Hotels;

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
        var itbisRate = new ItbisRate(0.18m);

        // Act
        var result = new Hotel(
            "Brisa Hospitality SRL",
            "Hotel Brisa",
            logoUrl,
            businessEmail,
            businessPhoneNumber,
            address,
            checkInOutTimes,
            itbisRate,
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
        result.ItbisRate.Rate.Should().Be(itbisRate.Rate);
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
            CreateItbisRate(),
            18m,
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
            CreateItbisRate(),
            18m,
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
            CreateItbisRate(),
            18m,
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
            CreateItbisRate(),
            18m,
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
            CreateItbisRate(),
            18m,
            true,
            invalidCurrency);

        // Assert
        act.Should().Throw<CurrencyNotSupportedException>();
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
            CreateItbisRate(),
            -1m,
            true);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateLegalName_ShouldUpdateLegalName_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();

        // Act
        hotel.UpdateLegalName("Brisa Resorts SRL");

        // Assert
        hotel.LegalName.Should().Be("Brisa Resorts SRL");
    }

    [Fact]
    public void UpdateLegalName_ShouldThrowEmptyRequiredFieldException_WhenValueIsWhiteSpace()
    {
        // Arrange
        var hotel = CreateHotel();

        // Act
        Action act = () => hotel.UpdateLegalName(" ");

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void UpdateCommercialName_ShouldUpdateCommercialName_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();

        // Act
        hotel.UpdateCommercialName("Brisa Beach");

        // Assert
        hotel.CommercialName.Should().Be("Brisa Beach");
    }

    [Fact]
    public void UpdateCommercialName_ShouldThrowEmptyRequiredFieldException_WhenValueIsWhiteSpace()
    {
        // Arrange
        var hotel = CreateHotel();

        // Act
        Action act = () => hotel.UpdateCommercialName(" ");

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void UpdateLogoUrl_ShouldUpdateLogoUrl_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();
        var newLogoUrl = new Url("https://cdn.example.com/hotel-logo.png");

        // Act
        hotel.UpdateLogoUrl(newLogoUrl);

        // Assert
        hotel.LogoUrl.Should().Be(newLogoUrl);
    }

    [Fact]
    public void UpdateBusinessEmail_ShouldUpdateBusinessEmail_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();
        var newBusinessEmail = new Email("frontdesk@hotelbrisa.com");

        // Act
        hotel.UpdateBusinessEmail(newBusinessEmail);

        // Assert
        hotel.BusinessEmail.Should().Be(newBusinessEmail);
    }

    [Fact]
    public void UpdateBusinessPhoneNumber_ShouldUpdateBusinessPhoneNumber_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();
        var newBusinessPhoneNumber = new PhoneNumber("+1 829 555 4321");

        // Act
        hotel.UpdateBusinessPhoneNumber(newBusinessPhoneNumber);

        // Assert
        hotel.BusinessPhoneNumber.Should().Be(newBusinessPhoneNumber);
    }

    [Fact]
    public void UpdateAddress_ShouldUpdateAddress_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();
        var newAddress = new Address("456 Ocean Drive", "Suite 8", "Punta Cana", "La Altagracia", "23000");

        // Act
        hotel.UpdateAddress(newAddress);

        // Assert
        hotel.Address.Should().Be(newAddress);
    }

    [Fact]
    public void UpdateCheckInOutTimes_ShouldUpdateCheckInOutTimes_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();
        var newCheckInOutTimes = new CheckInOutTimes(
            new DateTime(2026, 3, 28, 15, 0, 0),
            new DateTime(2026, 3, 29, 11, 0, 0));

        // Act
        hotel.UpdateCheckInOutTimes(newCheckInOutTimes);

        // Assert
        hotel.CheckInOutTimes.Should().Be(newCheckInOutTimes);
    }

    [Fact]
    public void UpdateDefaultCurrencyCode_ShouldUpdateDefaultCurrencyCode_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();

        // Act
        hotel.UpdateDefaultCurrencyCode(CurrencyCode.USD);

        // Assert
        hotel.DefaultCurrencyCode.Should().Be(CurrencyCode.USD);
    }

    [Fact]
    public void UpdateDefaultCurrencyCode_ShouldThrowCurrencyNotSupportedException_WhenValueIsInvalid()
    {
        // Arrange
        var hotel = CreateHotel();
        var invalidCurrency = (CurrencyCode)999;

        // Act
        Action act = () => hotel.UpdateDefaultCurrencyCode(invalidCurrency);

        // Assert
        act.Should().Throw<CurrencyNotSupportedException>();
    }

    [Fact]
    public void UpdateItbisRate_ShouldUpdateItbisRate_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();
        var newItbisRate = new ItbisRate(19m);

        // Act
        hotel.UpdateItbisRate(newItbisRate);

        // Assert
        hotel.ItbisRate.Rate.Should().Be(newItbisRate.Rate);
    }

    [Fact]
    public void UpdateItbisRate_ShouldThrowInvalidItbisRateException_WhenNewRateIsInvalid()
    {
        // Arrange
        var hotel = CreateHotel();
        var  newItbisRate = new ItbisRate(-19m);

        // Act
        Action act = () => hotel.UpdateItbisRate(newItbisRate);

        // Assert
        act.Should().Throw<InvalidItbisRateException>();
    }

    [Fact]
    public void UpdateServiceChargeRate_ShouldUpdateServiceChargeRate_WhenValueIsValid()
    {
        // Arrange
        var hotel = CreateHotel();

        // Act
        hotel.UpdateServiceChargeRate(12m);

        // Assert
        hotel.ServiceChargeRate.Should().Be(12m);
    }

    [Fact]
    public void UpdateServiceChargeRate_ShouldThrowBusinessRuleException_WhenValueIsNegative()
    {
        // Arrange
        var hotel = CreateHotel();

        // Act
        Action act = () => hotel.UpdateServiceChargeRate(-1m);

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
            CreateItbisRate(),
            18m,
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

    private static ItbisRate CreateItbisRate() => new ItbisRate(0.18m);
}
