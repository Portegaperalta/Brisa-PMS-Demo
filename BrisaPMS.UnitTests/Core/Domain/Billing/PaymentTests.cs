using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Billing;

public class PaymentTests
{
    [Fact]
    public void Constructor_ShouldCreatePayment_WhenValuesAreValid()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var guestId = Guid.NewGuid();
        var bookingId = Guid.NewGuid();
        var invoiceId = Guid.NewGuid();
        var amount = CreateAmount();

        // Act
        var result = new Payment
        (   
            hotelId,
            guestId,bookingId, 
            invoiceId, amount, 
            PaymentMethod.CreditCard,
            "AUTH-12345",
            "Paid at front desk"
        );

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
        result.HotelId.Should().Be(hotelId);
        result.GuestId.Should().Be(guestId);
        result.BookingId.Should().Be(bookingId);
        result.InvoiceId.Should().Be(invoiceId);
        result.Amount.Should().Be(amount);
        result.PaymentMethod.Should().Be(PaymentMethod.CreditCard);
        result.ReferenceNumber.Should().Be("AUTH-12345");
        result.Notes.Should().Be("Paid at front desk");
    }

    [Fact]
    public void Constructor_ShouldCreatePayment_WhenOptionalValuesAreNotProvided()
    {
        // Act
        var result = new Payment
            (
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                CreateAmount(),
                PaymentMethod.Cash
            );

        // Assert
        result.ReferenceNumber.Should().BeNull();
        result.Notes.Should().BeNull();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenHotelIdIsEmpty()
    {
        // Arrange
        var hotelId = Guid.Empty;

        // Act
        Action act = () => _ = CreatePayment(hotelId: hotelId);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenGuestIdIsEmpty()
    {
        // Arrange
        var guestId = Guid.Empty;

        // Act
        Action act = () => _ = CreatePayment(guestId: guestId);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenBookingIdIsEmpty()
    {
        // Arrange
        var bookingId = Guid.Empty;

        // Act
        Action act = () => _ = CreatePayment(bookingId: bookingId);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenInvoiceIdIsEmpty()
    {
        // Arrange
        var invoiceId = Guid.Empty;

        // Act
        Action act = () => _ = CreatePayment(invoiceId: invoiceId);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenPaymentMethodIsInvalid()
    {
        // Arrange
        var invalidPaymentMethod = (PaymentMethod)999;

        // Act
        Action act = () => _ = CreatePayment(paymentMethod: invalidPaymentMethod);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdateNotes_ShouldUpdateNotes_WhenValueIsValid()
    {
        // Arrange
        var payment = CreatePayment();

        // Act
        payment.UpdateNotes("Processed by night auditor");

        // Assert
        payment.Notes.Should().Be("Processed by night auditor");
    }

    [Fact]
    public void UpdateNotes_ShouldSetNotesToNull_WhenValueIsNull()
    {
        // Arrange
        var payment = CreatePayment();

        // Act
        payment.UpdateNotes(null!);

        // Assert
        payment.Notes.Should().BeNull();
    }

    private static Payment CreatePayment(
        Guid? hotelId = null,
        Guid? guestId = null,
        Guid? bookingId = null,
        Guid? invoiceId = null,
        PaymentMethod paymentMethod = PaymentMethod.CreditCard)
    {
        return new Payment(
            hotelId ?? Guid.NewGuid(),
            guestId ?? Guid.NewGuid(),
            bookingId ?? Guid.NewGuid(),
            invoiceId ?? Guid.NewGuid(),
            CreateAmount(),
            paymentMethod,
            "AUTH-12345",
            "Paid at front desk");
    }

    private static Money CreateAmount()
    {
        return new Money(150m);
    }
}
