using BrisaPMS.Domain.Entities;
using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Entities;

public class CompanyTests
{
    [Fact]
    public void Constructor_ShouldCreateCompany_WhenValuesAreValid()
    {
        // Arrange
        var rnc = CreateRnc();
        var businessEmail = CreateEmail();
        var businessPhone = CreatePhoneNumber();
        var logoUrl = CreateUrl();
        var address = CreateAddress();

        // Act
        var result = new Company("Brisa PMS SRL", "Brisa PMS", rnc, businessEmail, businessPhone, logoUrl, address);

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
        result.LegalName.Should().Be("Brisa PMS SRL");
        result.CommercialName.Should().Be("Brisa PMS");
        result.Rnc.Should().Be(rnc);
        result.BusinessEmail.Should().Be(businessEmail);
        result.BusinessPhone.Should().Be(businessPhone);
        result.LogoUrl.Should().Be(logoUrl);
        result.Address.Should().Be(address);
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenLegalNameIsNull()
    {
        // Arrange
        string legalName = null!;
        const string commercialName = "Brisa PMS";

        // Act
        Action act = () => _ = new Company(legalName, commercialName, CreateRnc(), CreateEmail(), CreatePhoneNumber(), CreateUrl(), CreateAddress());

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenLegalNameIsWhiteSpace()
    {
        // Arrange
        const string legalName = " ";
        const string commercialName = "Brisa PMS";

        // Act
        Action act = () => _ = new Company(legalName, commercialName, CreateRnc(), CreateEmail(), CreatePhoneNumber(), CreateUrl(), CreateAddress());

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenCommercialNameIsNull()
    {
        // Arrange
        const string legalName = "Brisa PMS SRL";
        string commercialName = null!;

        // Act
        Action act = () => _ = new Company(legalName, commercialName, CreateRnc(), CreateEmail(), CreatePhoneNumber(), CreateUrl(), CreateAddress());

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenCommercialNameIsWhiteSpace()
    {
        // Arrange
        const string legalName = "Brisa PMS SRL";
        const string commercialName = " ";

        // Act
        Action act = () => _ = new Company(legalName, commercialName, CreateRnc(), CreateEmail(), CreatePhoneNumber(), CreateUrl(), CreateAddress());

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void ChangeLegalName_ShouldUpdateLegalName_WhenValueIsValid()
    {
        // Arrange
        var company = CreateCompany();

        // Act
        company.ChangeLegalName("Brisa Technologies SRL");

        // Assert
        company.LegalName.Should().Be("Brisa Technologies SRL");
    }

    [Fact]
    public void ChangeCommercialName_ShouldUpdateCommercialName_WhenValueIsValid()
    {
        // Arrange
        var company = CreateCompany();

        // Act
        company.ChangeCommercialName("Brisa Tech");

        // Assert
        company.CommercialName.Should().Be("Brisa Tech");
    }

    [Fact]
    public void ChangeRnc_ShouldUpdateRnc_WhenValueIsValid()
    {
        // Arrange
        var company = CreateCompany();
        var newRnc = new Rnc("00112345678");

        // Act
        company.ChangeRnc(newRnc);

        // Assert
        company.Rnc.Should().Be(newRnc);
    }

    [Fact]
    public void ChangeBusinessEmail_ShouldUpdateBusinessEmail_WhenValueIsValid()
    {
        // Arrange
        var company = CreateCompany();
        var newBusinessEmail = new Email("support@brisapms.com");

        // Act
        company.ChangeBusinessEmail(newBusinessEmail);

        // Assert
        company.BusinessEmail.Should().Be(newBusinessEmail);
    }

    [Fact]
    public void ChangeBusinessPhone_ShouldUpdateBusinessPhone_WhenValueIsValid()
    {
        // Arrange
        var company = CreateCompany();
        var newBusinessPhone = new PhoneNumber("+1 829 555 4321");

        // Act
        company.ChangeBusinessPhone(newBusinessPhone);

        // Assert
        company.BusinessPhone.Should().Be(newBusinessPhone);
    }

    [Fact]
    public void ChangeLogoUrl_ShouldUpdateLogoUrl_WhenValueIsValid()
    {
        // Arrange
        var company = CreateCompany();
        var newLogoUrl = new Url("https://cdn.example.com/logo.png");

        // Act
        company.ChangeLogoUrl(newLogoUrl);

        // Assert
        company.LogoUrl.Should().Be(newLogoUrl);
    }

    [Fact]
    public void ChangeAddress_ShouldUpdateAddress_WhenValueIsValid()
    {
        // Arrange
        var company = CreateCompany();
        var newAddress = new Address("456 Ocean Drive", "Suite 8", "Punta Cana", "La Altagracia", "23000");

        // Act
        company.ChangeAddress(newAddress);

        // Assert
        company.Address.Should().Be(newAddress);
    }

    [Fact]
    public void ChangeLegalName_ShouldThrowEmptyRequiredFieldException_WhenNameIsWhiteSpace()
    {
        // Arrange
        var company = CreateCompany();
        const string value = " ";

        // Act
        Action act = () => company.ChangeLegalName(value);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void ChangeCommercialName_ShouldThrowEmptyRequiredFieldException_WhenNameIsWhiteSpace()
    {
        // Arrange
        var company = CreateCompany();
        const string value = " ";

        // Act
        Action act = () => company.ChangeCommercialName(value);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    private static Company CreateCompany()
    {
        return new Company("Brisa PMS SRL", "Brisa PMS", CreateRnc(), CreateEmail(), CreatePhoneNumber(), CreateUrl(), CreateAddress());
    }

    private static Rnc CreateRnc()
    {
        return new Rnc("123456789");
    }

    private static Email CreateEmail()
    {
        return new Email("contact@brisapms.com");
    }

    private static PhoneNumber CreatePhoneNumber()
    {
        return new PhoneNumber("+1 809 555 1234");
    }

    private static Url CreateUrl()
    {
        return new Url("https://example.com/logo.png");
    }

    private static Address CreateAddress()
    {
        return new Address("123 Main Street", "Suite 4B", "Santo Domingo", "Distrito Nacional", "10101");
    }
}
