using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using BrisaPMS.Domain.Users;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Users;

public class UserTests
{
    [Fact]
    public void Builder_ShouldCreateUser_WhenValuesAreValid()
    {
        // Arrange
        var role = UserRole.Admin;
        var hotelId = Guid.NewGuid();
        var email = CreateEmail();
        var phoneNumber = CreatePhoneNumber();

        // Act
        var result = new User.Builder
        (
            role,
            "John",
            "Doe",
            email,
            UserPreferredLanguage.En
        )
        .WithHotelId(hotelId)
        .WithPhoneNumber(phoneNumber)
        .Build();

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
        result.Role.Should().Be(role);
        result.HotelId.Should().Be(hotelId);
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.Email.Should().Be(email);
        result.PhoneNumber.Should().Be(phoneNumber);
        result.PreferredLanguage.Should().Be(UserPreferredLanguage.En);
        result.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Builder_ShouldCreateUser_WhenPhoneNumberIsNotProvided()
    {
        // Arrange
        var role = UserRole.Manager;
        var hotelId = Guid.NewGuid();

        // Act
        var result = new User.Builder
        (
            role,
            "John",
            "Doe",
            CreateEmail(),
            UserPreferredLanguage.En
        )
        .WithHotelId(hotelId)
        .Build();

        // Assert
        result.Role.Should().Be(role);
        result.HotelId.Should().Be(hotelId);
        result.PhoneNumber.Should().BeNull();
    }

    [Fact]
    public void Builder_ShouldCreateUser_WhenHotelIdIsNotProvided()
    {
        // Arrange + Act
        var result = new User.Builder
        (
            UserRole.Admin,
            "John",
            "Doe",
            CreateEmail(),
            UserPreferredLanguage.En
        )
        .Build();

        // Assert
        result.HotelId.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    public void Builder_ShouldThrowEmptyRequiredFieldException_WhenFirstNameIsNullOrWhiteSpace(string? firstName)
    {
        // Act
        Action act = () => _ = new User.Builder
        (
            UserRole.Admin,
            firstName!,
            "Doe",
            CreateEmail(),
            UserPreferredLanguage.En
        );

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    public void Builder_ShouldThrowEmptyRequiredFieldException_WhenLastNameIsNullOrWhiteSpace(string? lastName)
    {
        // Act
        Action act = () => _ = new User.Builder
        (
            UserRole.Admin,
            "John",
            lastName!,
            CreateEmail(),
            UserPreferredLanguage.En
        );

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Builder_ShouldThrowLanguageNotSupportedException_WhenPreferredLanguageIsInvalid()
    {
        // Arrange
        var invalidLanguage = (UserPreferredLanguage)999;

        // Act
        Action act = () => _ = new User.Builder
        (
            UserRole.Admin,
            "John",
            "Doe",
            CreateEmail(),
            invalidLanguage
        );

        // Assert
        act.Should().Throw<LanguageNotSupportedException>();
    }

    [Fact]
    public void ChangeRole_ShouldUpdateRole_WhenRoleIsValid()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.ChangeRole(UserRole.Manager);

        // Assert
        user.Role.Should().Be(UserRole.Manager);
    }

    [Fact]
    public void ChangeRole_ShouldThrowBusinessRuleException_WhenUserRoleIsAdmin()
    {
        // Arrange
        var user = CreateUser(UserRole.Admin);

        // Act
        Action act = () => user.ChangeRole(UserRole.Manager);

        // Assert
        act.Should().Throw<BusinessRuleException>();
        user.Role.Should().Be(UserRole.Admin);
    }

    [Fact]
    public void UpdateFirstName_ShouldUpdateFirstName_WhenNewFirstNameIsValid()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.UpdateFirstName("Jane");

        // Assert
        user.FirstName.Should().Be("Jane");
    }

    [Fact]
    public void UpdateFirstName_ShouldThrowEmptyRequiredFieldException_WhenNewFirstNameIsWhiteSpace()
    {
        // Arrange
        var user = CreateUser();

        // Act
        Action act = () => user.UpdateFirstName(" ");

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void UpdateLastName_ShouldUpdateLastName_WhenNewLastNameIsValid()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.UpdateLastName("Smith");

        // Assert
        user.LastName.Should().Be("Smith");
    }

    [Fact]
    public void UpdateLastName_ShouldThrowEmptyRequiredFieldException_WhenNewLastNameIsWhiteSpace()
    {
        // Arrange
        var user = CreateUser();

        // Act
        Action act = () => user.UpdateLastName(" ");

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void ChangeEmail_ShouldUpdateEmail_WhenEmailIsValid()
    {
        // Arrange
        var user = CreateUser();
        var newEmail = new Email("jane.doe@example.com");

        // Act
        user.ChangeEmail(newEmail);

        // Assert
        user.Email.Should().Be(newEmail);
    }

    [Fact]
    public void ChangePhoneNumber_ShouldUpdatePhoneNumber_WhenPhoneNumberIsValid()
    {
        // Arrange
        var user = CreateUser();
        var newPhoneNumber = new PhoneNumber("+1 829 555 4321");

        // Act
        user.ChangePhoneNumber(newPhoneNumber);

        // Assert
        user.PhoneNumber.Should().Be(newPhoneNumber);
    }

    [Fact]
    public void UpdatePreferredLanguage_ShouldUpdatePreferredLanguage_WhenLanguageIsValid()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.UpdatePreferredLanguage(UserPreferredLanguage.Es);

        // Assert
        user.PreferredLanguage.Should().Be(UserPreferredLanguage.Es);
    }

    [Fact]
    public void UpdatePreferredLanguage_ShouldThrowLanguageNotSupportedException_WhenLanguageIsInvalid()
    {
        // Arrange
        var user = CreateUser();
        var invalidLanguage = (UserPreferredLanguage)999;

        // Act
        Action act = () => user.UpdatePreferredLanguage(invalidLanguage);

        // Assert
        act.Should().Throw<LanguageNotSupportedException>();
    }

    private static User CreateUser(UserRole role = UserRole.Receptionist)
    {
        return new User.Builder
        (
            role,
            "John",
            "Doe",
            CreateEmail(),
            UserPreferredLanguage.En
        )
        .WithHotelId(Guid.NewGuid())
        .WithPhoneNumber(CreatePhoneNumber())
        .Build();
    }

    private static Email CreateEmail()
    {
        return new Email("john.doe@example.com");
    }

    private static PhoneNumber CreatePhoneNumber()
    {
        return new PhoneNumber("+1 809 555 1234");
    }
}
