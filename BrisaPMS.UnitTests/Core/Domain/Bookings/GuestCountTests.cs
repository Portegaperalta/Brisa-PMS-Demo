using BrisaPMS.Domain.Bookings;
using BrisaPMS.Domain.Shared.Exceptions;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Bookings;

public class GuestCountTests
{
    [Fact]
    public void Constructor_ShouldCreateGuestCount_WhenValuesAreValid()
    {
        // Arrange
        const int numberOfAdults = 2;
        const int numberOfChildren = 1;

        // Act
        var result = new GuestCount(numberOfAdults, numberOfChildren);

        // Assert
        result.NumberOfAdults.Should().Be(numberOfAdults);
        result.NumberOfChildren.Should().Be(numberOfChildren);
    }

    [Fact]
    public void Constructor_ShouldCreateGuestCount_WhenValuesAreAtUpperBoundary()
    {
        // Arrange
        const int numberOfAdults = 10;
        const int numberOfChildren = 10;

        // Act
        var result = new GuestCount(numberOfAdults, numberOfChildren);

        // Assert
        result.NumberOfAdults.Should().Be(numberOfAdults);
        result.NumberOfChildren.Should().Be(numberOfChildren);
    }

    [Fact]
    public void Constructor_ShouldCreateGuestCount_WhenChildrenIsZero()
    {
        // Arrange
        const int numberOfAdults = 2;
        const int numberOfChildren = 0;

        // Act
        var result = new GuestCount(numberOfAdults, numberOfChildren);

        // Assert
        result.NumberOfAdults.Should().Be(numberOfAdults);
        result.NumberOfChildren.Should().Be(numberOfChildren);
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenNumberOfAdultsIsZeroOrLess()
    {
        // Arrange
        const int numberOfAdults = 0;
        const int numberOfChildren = 1;

        // Act
        Action act = () => _ = new GuestCount(numberOfAdults, numberOfChildren);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenNumberOfAdultsIsGreaterThan10()
    {
        // Arrange
        const int numberOfAdults = 11;
        const int numberOfChildren = 1;

        // Act
        Action act = () => _ = new GuestCount(numberOfAdults, numberOfChildren);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenNumberOfChildrenIsNegative()
    {
        // Arrange
        const int numberOfAdults = 2;
        const int numberOfChildren = -1;

        // Act
        Action act = () => _ = new GuestCount(numberOfAdults, numberOfChildren);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenNumberOfChildrenIsGreaterThan10()
    {
        // Arrange
        const int numberOfAdults = 2;
        const int numberOfChildren = 11;

        // Act
        Action act = () => _ = new GuestCount(numberOfAdults, numberOfChildren);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }
}
