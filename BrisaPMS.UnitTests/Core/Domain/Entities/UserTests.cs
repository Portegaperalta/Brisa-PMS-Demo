using BrisaPMS.Domain.Entities;
using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.ValueObjects;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Entities;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldCreateUser_WhenValuesAreValid()
    {
        // Arrange
        var role = Role.Admin;
        var hotelId = Guid.NewGuid();
        var email = CreateEmail();
        var password = CreatePassword();
        var phoneNumber = CreatePhoneNumber();

        // Act
        var result = new User(role, hotelId, "John", "Doe", email, password, PreferredLanguage.En, phoneNumber);

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
        result.Role.Should().Be(role);
        result.HotelId.Should().Be(hotelId);
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.Email.Should().Be(email);
        result.PasswordHash.Should().Be(password);
        result.PhoneNumber.Should().Be(phoneNumber);
        result.PreferredLanguage.Should().Be(PreferredLanguage.En);
        result.IsOnline.Should().BeFalse();
        result.IsActive.Should().BeTrue();
        result.IsEmailConfirmed.Should().BeFalse();
        result.FailedLoginAttempts.Should().Be(0);
        result.LockOutDuration.Should().BeNull();
        result.LockOutEnd.Should().BeNull();
        result.LastLoginAt.Should().BeNull();
        result.PasswordChangedAt.Should().BeNull();
    }

    [Fact]
    public void Constructor_ShouldCreateUser_WhenPhoneNumberIsNotProvided()
    {
        // Arrange
        var role = Role.Manager;
        var hotelId = Guid.NewGuid();

        // Act
        var result = new User(role, hotelId, "John", "Doe", CreateEmail(), CreatePassword(), PreferredLanguage.En);

        // Assert
        result.Role.Should().Be(role);
        result.HotelId.Should().Be(hotelId);
        result.PhoneNumber.Should().BeNull();
    }

    [Fact]
    public void Constructor_ShouldCreateUser_WhenHotelIdIsNull()
    {
        // Arrange + Act
        var result = new User(Role.Admin, null, "John", "Doe", CreateEmail(), CreatePassword(), PreferredLanguage.En);

        // Assert
        result.HotelId.Should().BeNull();
    }

    [Theory]
    [InlineData("First Name")]
    [InlineData("Last Name")]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenRequiredNameIsNullOrWhiteSpace(string fieldName)
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";

        if (fieldName == "First Name")
            firstName = " ";
        else
            lastName = " ";

        // Act
        Action act = () => _ = new User(Role.Admin, Guid.NewGuid(), firstName, lastName, CreateEmail(), CreatePassword(), PreferredLanguage.En);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowLanguageNotSupportedException_WhenPreferredLanguageIsInvalid()
    {
        // Arrange
        var invalidLanguage = (PreferredLanguage)999;

        // Act
        Action act = () => _ = new User(Role.Admin, Guid.NewGuid(), "John", "Doe", CreateEmail(), CreatePassword(), invalidLanguage);

        // Assert
        act.Should().Throw<LanguageNotSupportedException>();
    }

    [Fact]
    public void ChangeRole_ShouldUpdateRole_WhenRoleIsValid()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.ChangeRole(Role.Manager);

        // Assert
        user.Role.Should().Be(Role.Manager);
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
    public void ChangePassword_ShouldUpdatePasswordHash_WhenPasswordIsValid()
    {
        // Arrange
        var user = CreateUser();
        var newPassword = new Password("NewPassword123!");

        // Act
        user.ChangePassword(newPassword);

        // Assert
        user.PasswordHash.Should().Be(newPassword);
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
        user.UpdatePreferredLanguage(PreferredLanguage.Es);

        // Assert
        user.PreferredLanguage.Should().Be(PreferredLanguage.Es);
    }

    [Fact]
    public void UpdatePreferredLanguage_ShouldThrowLanguageNotSupportedException_WhenLanguageIsInvalid()
    {
        // Arrange
        var user = CreateUser();
        var invalidLanguage = (PreferredLanguage)999;

        // Act
        Action act = () => user.UpdatePreferredLanguage(invalidLanguage);

        // Assert
        act.Should().Throw<LanguageNotSupportedException>();
    }

    [Fact]
    public void EnableOnlineStatus_ShouldSetIsOnlineToTrue()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.EnableOnlineStatus();

        // Assert
        user.IsOnline.Should().BeTrue();
    }

    [Fact]
    public void DisableOnlineStatus_ShouldSetIsOnlineToFalse()
    {
        // Arrange
        var user = CreateUser();
        user.EnableOnlineStatus();

        // Act
        user.DisableOnlineStatus();

        // Assert
        user.IsOnline.Should().BeFalse();
    }

    [Fact]
    public void SetEmailAsConfirmed_ShouldMarkEmailAsConfirmed()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.SetEmailAsConfirmed();

        // Assert
        user.IsEmailConfirmed.Should().BeTrue();
    }

    [Fact]
    public void IncreaseFailedLoginAttempts_ShouldIncrementFailedLoginAttempts()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.IncreaseFailedLoginAttempts();
        user.IncreaseFailedLoginAttempts();

        // Assert
        user.FailedLoginAttempts.Should().Be(2);
    }

    [Fact]
    public void SetLockoutDuration_ShouldUpdateLockoutDuration()
    {
        // Arrange
        var user = CreateUser();
        var lockoutDuration = TimeSpan.FromMinutes(15);

        // Act
        user.SetLockoutDuration(lockoutDuration);

        // Assert
        user.LockOutDuration.Should().Be(lockoutDuration);
    }

    [Fact]
    public void SetLockoutEnd_ShouldUpdateLockoutEnd_WhenDateIsInTheFuture()
    {
        // Arrange
        var user = CreateUser();
        var lockoutEnd = DateTimeOffset.UtcNow.AddMinutes(10);

        // Act
        user.SetLockoutEnd(lockoutEnd);

        // Assert
        user.LockOutEnd.Should().Be(lockoutEnd);
    }

    [Fact]
    public void SetLockoutEnd_ShouldThrowExpiredLockOutEndDateException_WhenDateIsInThePast()
    {
        // Arrange
        var user = CreateUser();
        var expiredLockoutEnd = DateTimeOffset.UtcNow.AddMinutes(-1);

        // Act
        Action act = () => user.SetLockoutEnd(expiredLockoutEnd);

        // Assert
        act.Should().Throw<ExpiredLockOutEndDateException>();
    }

    [Fact]
    public void TimestampUpdateMethods_ShouldSetAuditDates()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.UpdateLastLoginTime();
        user.UpdatedLastPasswordChangeTime();

        // Assert
        user.LastLoginAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        user.PasswordChangedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    private static User CreateUser()
    {
        return new User(Role.Receptionist, Guid.NewGuid(), "John", "Doe", CreateEmail(), CreatePassword(), PreferredLanguage.En, CreatePhoneNumber());
    }

    private static Email CreateEmail()
    {
        return new Email("john.doe@example.com");
    }

    private static Password CreatePassword()
    {
        return new Password("Password123!");
    }

    private static PhoneNumber CreatePhoneNumber()
    {
        return new PhoneNumber("+1 809 555 1234");
    }
}
