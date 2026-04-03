using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Billing;

public class InvoiceTests
{
    [Fact]
    public void Constructor_ShouldCreateInvoice_WhenValuesAreValid()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var stayId = Guid.NewGuid();
        var issuedBy = Guid.NewGuid();
        var ncf = CreateNcf();
        var guestRnc = CreateRnc();
        var exchangeRate = CreateMoney(1m);
        var roomCharges = CreateMoney(200m);
        var servicesTotal = CreateMoney(50m);
        var subTotal = CreateMoney(250m);
        var discountTotal = CreateMoney(10m);
        var netTotal = CreateMoney(240m);
        var itbisAmount = CreateMoney(43.2m);
        var legalTipTotal = CreateMoney(24m);
        var paidAmount = CreateMoney(240m);

        // Act
        var result = new Invoice(
            hotelId,
            stayId,
            issuedBy,
            ncf,
            InvoiceType.Guest,
            CurrencyCode.DOP,
            exchangeRate,
            roomCharges,
            servicesTotal,
            subTotal,
            discountTotal,
            netTotal,
            itbisAmount,
            legalTipTotal,
            paidAmount,
            InvoiceStatus.Paid,
            guestRnc);

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
        result.HotelId.Should().Be(hotelId);
        result.StayId.Should().Be(stayId);
        result.IssuedBy.Should().Be(issuedBy);
        result.Ncf.Should().Be(ncf);
        result.Type.Should().Be(InvoiceType.Guest);
        result.GuestRnc.Should().Be(guestRnc);
        result.CurrencyCode.Should().Be(CurrencyCode.DOP);
        result.ExchangeRate.Should().Be(exchangeRate);
        result.RoomCharges.Should().Be(roomCharges);
        result.ServicesTotal.Should().Be(servicesTotal);
        result.SubTotal.Should().Be(subTotal);
        result.DiscountTotal.Should().Be(discountTotal);
        result.NetTotal.Should().Be(netTotal);
        result.ItbisAmount.Should().Be(itbisAmount);
        result.LegalTipTotal.Should().Be(legalTipTotal);
        result.PaidAmount.Should().Be(paidAmount);
        result.Status.Should().Be(InvoiceStatus.Paid);
        result.IssuedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Constructor_ShouldCreateInvoice_WhenGuestRncIsNotProvided()
    {
        // Arrange
        var netTotal = CreateMoney(240m);

        // Act
        var result = new Invoice(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            CreateNcf(),
            InvoiceType.Group,
            CurrencyCode.DOP,
            CreateMoney(1m),
            CreateMoney(200m),
            CreateMoney(50m),
            CreateMoney(250m),
            CreateMoney(10m),
            netTotal,
            CreateMoney(43.2m),
            CreateMoney(24m),
            netTotal,
            InvoiceStatus.Sent);

        // Assert
        result.GuestRnc.Should().BeNull();
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenHotelIdIsEmpty()
    {
        // Arrange
        var hotelId = Guid.Empty;

        // Act
        Action act = () => _ = CreateInvoice(hotelId: hotelId);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenStayIdIsEmpty()
    {
        // Arrange
        var stayId = Guid.Empty;

        // Act
        Action act = () => _ = CreateInvoice(stayId: stayId);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenIssuedByIsEmpty()
    {
        // Arrange
        var issuedBy = Guid.Empty;

        // Act
        Action act = () => _ = CreateInvoice(issuedBy: issuedBy);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenTypeIsInvalid()
    {
        // Arrange
        var invalidType = (InvoiceType)999;

        // Act
        Action act = () => _ = CreateInvoice(type: invalidType);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenCurrencyCodeIsInvalid()
    {
        // Arrange
        var invalidCurrencyCode = (CurrencyCode)999;

        // Act
        Action act = () => _ = CreateInvoice(currencyCode: invalidCurrencyCode);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenPaidAmountIsLessThanNetTotal()
    {
        // Arrange
        var netTotal = CreateMoney(240m);
        var paidAmount = CreateMoney(239m);

        // Act
        Action act = () => _ = CreateInvoice(netTotal: netTotal, paidAmount: paidAmount);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    private static Invoice CreateInvoice(
        Guid? hotelId = null,
        Guid? stayId = null,
        Guid? issuedBy = null,
        InvoiceType type = InvoiceType.Guest,
        CurrencyCode currencyCode = CurrencyCode.DOP,
        Money? netTotal = null,
        Money? paidAmount = null)
    {
        var currentNetTotal = netTotal ?? CreateMoney(240m);

        return new Invoice(
            hotelId ?? Guid.NewGuid(),
            stayId ?? Guid.NewGuid(),
            issuedBy ?? Guid.NewGuid(),
            CreateNcf(),
            type,
            currencyCode,
            CreateMoney(1m),
            CreateMoney(200m),
            CreateMoney(50m),
            CreateMoney(250m),
            CreateMoney(10m),
            currentNetTotal,
            CreateMoney(43.2m),
            CreateMoney(24m),
            paidAmount ?? currentNetTotal,
            InvoiceStatus.Paid,
            CreateRnc());
    }

    private static Ncf CreateNcf()
    {
        return new Ncf("B01-00000000001");
    }

    private static Rnc CreateRnc()
    {
        return new Rnc("101123456");
    }

    private static Money CreateMoney(decimal amount)
    {
        return new Money(amount, CurrencyCode.DOP);
    }
}
