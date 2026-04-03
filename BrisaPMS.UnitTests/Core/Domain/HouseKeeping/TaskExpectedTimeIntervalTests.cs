using BrisaPMS.Domain.HouseKeeping;
using BrisaPMS.Domain.Shared.Exceptions;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.HouseKeeping;

public class TaskExpectedTimeIntervalTests
{
    [Fact]
    public void Constructor_ShouldCreateTaskExpectedTimeInterval_WhenExpectedStartAtIsBeforeExpectedEndAt()
    {
        // Arrange
        var expectedStartAt = new DateTime(2026, 4, 1, 9, 0, 0);
        var expectedEndAt = new DateTime(2026, 4, 1, 10, 0, 0);

        // Act
        var result = new TaskExpectedTimeInterval(expectedStartAt, expectedEndAt);

        // Assert
        result.ExpectedStartAt.Should().Be(expectedStartAt);
        result.ExpectedEndAt.Should().Be(expectedEndAt);
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenExpectedStartAtIsAfterExpectedEndAt()
    {
        // Arrange
        var expectedStartAt = new DateTime(2026, 4, 1, 11, 0, 0);
        var expectedEndAt = new DateTime(2026, 4, 1, 10, 0, 0);

        // Act
        Action act = () => _ = new TaskExpectedTimeInterval(expectedStartAt, expectedEndAt);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenExpectedStartAtIsEqualToExpectedEndAt()
    {
        // Arrange
        var expectedStartAt = new DateTime(2026, 4, 1, 10, 0, 0);
        var expectedEndAt = expectedStartAt;

        // Act
        Action act = () => _ = new TaskExpectedTimeInterval(expectedStartAt, expectedEndAt);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }
}
