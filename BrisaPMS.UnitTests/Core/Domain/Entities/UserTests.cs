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
        var email = CreateEmail();
        var password = CreatePassword();
        var phoneNumber = CreatePhoneNumber();

        // Act
        var result = new User("John", "Doe", email, password, phoneNumber, PreferredLanguage.En);

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.Email.Should().Be(email);
        result.PasswordHash.Should().Be(password);
        result.PhoneNumber.Should().Be(phoneNumber);
        result.PreferredLanguage.Should().Be(PreferredLanguage.En);
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
        Action act = () => _ = new User(firstName, lastName, CreateEmail(), CreatePassword(), CreatePhoneNumber(), PreferredLanguage.En);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Theory]
    [InlineData("First Name", 251)]
    [InlineData("Last Name", 251)]
    public void Constructor_ShouldThrowMaxCharacterLimitException_WhenNameExceedsMaxLength(string fieldName, int length)
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";

        if (fieldName == "First Name")
            firstName = new string('a', length);
        else
            lastName = new string('a', length);

        // Act
        Action act = () => _ = new User(firstName, lastName, CreateEmail(), CreatePassword(), CreatePhoneNumber(), PreferredLanguage.En);

        // Assert
        act.Should().Throw<MaxCharacterLimitException>();
    }

    [Fact]
    public void Constructor_ShouldThrowLanguageNotSupportedException_WhenPreferredLanguageIsInvalid()
    {
        // Arrange
        var invalidLanguage = (PreferredLanguage)999;

        // Act
        Action act = () => _ = new User("John", "Doe", CreateEmail(), CreatePassword(), CreatePhoneNumber(), invalidLanguage);

        // Assert
        act.Should().Throw<LanguageNotSupportedException>();
    }

    [Fact]
    public void ChangeFirstName_ShouldUpdateFirstName_WhenNewFirstNameIsValid()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.ChangeFirstName("Jane");

        // Assert
        user.FirstName.Should().Be("Jane");
    }

    [Fact]
    public void ChangeLastName_ShouldUpdateLastName_WhenNewLastNameIsValid()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.ChangeLastName("Smith");

        // Assert
        user.LastName.Should().Be("Smith");
    }

    [Fact]
    public void ChangePreferredLanguage_ShouldUpdatePreferredLanguage_WhenLanguageIsValid()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.ChangePreferredLanguage(PreferredLanguage.Es);

        // Assert
        user.PreferredLanguage.Should().Be(PreferredLanguage.Es);
    }

    [Fact]
    public void ChangePreferredLanguage_ShouldThrowLanguageNotSupportedException_WhenLanguageIsInvalid()
    {
        // Arrange
        var user = CreateUser();
        var invalidLanguage = (PreferredLanguage)999;

        // Act
        Action act = () => user.ChangePreferredLanguage(invalidLanguage);

        // Assert
        act.Should().Throw<LanguageNotSupportedException>();
    }

    [Fact]
    public void ChangeContactAndCredentialData_ShouldUpdateReferenceProperties()
    {
        // Arrange
        var user = CreateUser();
        var newEmail = new Email("jane.doe@example.com");
        var newPassword = new Password("NewPassword123!");
        var newPhoneNumber = new PhoneNumber("+1 829 555 4321");

        // Act
        user.ChangeEmail(newEmail);
        user.ChangePassword(newPassword);
        user.ChangePhoneNumber(newPhoneNumber);

        // Assert
        user.Email.Should().Be(newEmail);
        user.PasswordHash.Should().Be(newPassword);
        user.PhoneNumber.Should().Be(newPhoneNumber);
    }

    [Fact]
    public void OnlineStatusMethods_ShouldToggleOnlineStatus()
    {
        // Arrange
        var user = CreateUser();

        // Act
        user.EnableOnlineStatus();
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
        user.UpdatedLastProfileUpdateTime();

        // Assert
        user.LastLoginAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        user.PasswordChangedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    private static User CreateUser()
    {
        return new User("John", "Doe", CreateEmail(), CreatePassword(), CreatePhoneNumber(), PreferredLanguage.En);
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
