using BrisaPMS.Domain.Entities;
using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.Exceptions.EmptyValueExceptions;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Entities
{
    public class UserUnitTests
    {
        [Fact]
        public void Constructor_ShouldCreateUserWithExpectedDefaultState()
        {
            // Arrange
            var firstName = "Jane";
            var lastName = "Doe";
            var email = "jane.doe@email.com";
            var passwordHash = "hashed-password";
            var phoneNumber = "8095551234";
            var preferredLanguage = "es";

            // Act
            var user = new User(firstName, lastName, email, passwordHash, phoneNumber, preferredLanguage);

            // Assert
            user.FirstName.Should().Be(firstName);
            user.LastName.Should().Be(lastName);
            user.Email.Should().Be(email);
            user.PasswordHash.Should().Be(passwordHash);
            user.PhoneNumber.Should().Be(phoneNumber);
            user.PreferredLanguage.Should().Be(preferredLanguage);
            user.IsOnline.Should().BeFalse();
            user.IsActive.Should().BeTrue();
            user.IsEmailConfirmed.Should().BeFalse();
            user.FailedLoginAttempts.Should().Be(0);
            user.LockOutDuration.Should().BeNull();
            user.LockOutEnd.Should().BeNull();
            user.LastLoginAt.Should().BeNull();
            user.PasswordChangedAt.Should().BeNull();
            user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
            user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }

        [Fact]
        public void Constructor_ShouldAllowNullPhoneNumber()
        {
            // Arrange
            string? phoneNumber = null;

            // Act
            var user = new User("Jane", "Doe", "jane.doe@email.com", "hashed-password", phoneNumber, "en");

            // Assert
            user.PhoneNumber.Should().BeNull();
        }

        [Fact]
        public void Constructor_ShouldRespectProvidedIsActiveValue()
        {
            // Arrange
            const bool isActive = false;

            // Act
            var user = new User("Jane", "Doe", "jane.doe@email.com", "hashed-password", "8095551234", "en", isActive);

            // Assert
            user.IsActive.Should().BeFalse();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void Constructor_ShouldThrowEmptyFirstNameException_WhenFirstNameIsInvalid(string? invalidFirstName)
        {
            // Arrange + Act
            Action act = () => new User(invalidFirstName!, "Doe", "jane.doe@email.com", "hashed-password", "8095551234", "en");

            // Assert
            act.Should().Throw<EmptyFirstNameException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void Constructor_ShouldThrowEmptyLastNameException_WhenLastNameIsInvalid(string? invalidLastName)
        {
            // Arrange + Act
            Action act = () => new User("Jane", invalidLastName!, "jane.doe@email.com", "hashed-password", "8095551234", "en");

            // Assert
            act.Should().Throw<EmptyLastNameException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void Constructor_ShouldThrowEmptyEmailException_WhenEmailIsInvalid(string? invalidEmail)
        {
            // Arrange + Act
            Action act = () => new User("Jane", "Doe", invalidEmail!, "hashed-password", "8095551234", "en");

            // Assert
            act.Should().Throw<EmptyEmailException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void Constructor_ShouldThrowEmptyPasswordHashException_WhenPasswordHashIsInvalid(string? invalidPasswordHash)
        {
            // Arrange + Act
            Action act = () => new User("Jane", "Doe", "jane.doe@email.com", invalidPasswordHash!, "8095551234", "en");

            // Assert
            act.Should().Throw<EmptyPasswordException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void
        Constructor_ShouldThrowEmptyPreferredLanguageException_WhenPreferredLanguageIsInvalid(string? invalidPreferredLanguage)
        {
            // Arrange + Act
            Action act = () => new User("Jane", "Doe", "jane.doe@email.com", "hashed-password", "8095551234", invalidPreferredLanguage!);

            // Assert
            act.Should().Throw<EmptyPreferredLanguageException>();
        }

        [Fact]
        public void ChangeFirstName_ShouldUpdateFirstName_WhenValueIsValid()
        {
            // Arrange
            var user = CreateUser();
            var newFirstName = "Janet";

            // Act
            user.ChangeFirstName(newFirstName);

            // Assert
            user.FirstName.Should().Be(newFirstName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void ChangeFirstName_ShouldThrowEmptyFirstNameException_WhenValueIsInvalid(string? invalidFirstName)
        {
            // Arrange
            var user = CreateUser();

            // Act
            Action act = () => user.ChangeFirstName(invalidFirstName!);

            // Assert
            act.Should().Throw<EmptyFirstNameException>();
        }

        [Fact]
        public void ChangeLastName_ShouldUpdateLastName_WhenValueIsValid()
        {
            // Arrange
            var user = CreateUser();
            var newLastName = "Smith";

            // Act
            user.ChangeLastName(newLastName);

            // Assert
            user.LastName.Should().Be(newLastName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void ChangeLastName_ShouldThrowEmptyLastNameException_WhenValueIsInvalid(string? invalidLastName)
        {
            // Arrange
            var user = CreateUser();

            // Act
            Action act = () => user.ChangeLastName(invalidLastName!);

            // Assert
            act.Should().Throw<EmptyLastNameException>();
        }

        [Fact]
        public void ChangeEmail_ShouldUpdateEmail_WhenValueIsValid()
        {
            // Arrange
            var user = CreateUser();
            var newEmail = "janet.smith@email.com";

            // Act
            user.ChangeEmail(newEmail);

            // Assert
            user.Email.Should().Be(newEmail);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void ChangeEmail_ShouldThrowEmptyEmailException_WhenValueIsInvalid(string? invalidEmail)
        {
            // Arrange
            var user = CreateUser();

            // Act
            Action act = () => user.ChangeEmail(invalidEmail!);

            // Assert
            act.Should().Throw<EmptyEmailException>();
        }

        [Fact]
        public void ChangePasswordHash_ShouldUpdatePasswordHash_WhenValueIsValid()
        {
            // Arrange
            var user = CreateUser();
            var newPasswordHash = "new-hashed-password";

            // Act
            user.ChangePasswordHash(newPasswordHash);

            // Assert
            user.PasswordHash.Should().Be(newPasswordHash);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void ChangePasswordHash_ShouldThrowEmptyPasswordHashException_WhenValueIsInvalid(string? invalidPasswordHash)
        {
            // Arrange
            var user = CreateUser();

            // Act
            Action act = () => user.ChangePasswordHash(invalidPasswordHash!);

            // Assert
            act.Should().Throw<EmptyPasswordException>();
        }

        [Fact]
        public void ChangePhoneNumber_ShouldUpdatePhoneNumber_WhenValueIsValid()
        {
            // Arrange
            var user = CreateUser();
            var newPhoneNumber = "8295559876";

            // Act
            user.ChangePhoneNumber(newPhoneNumber);

            // Assert
            user.PhoneNumber.Should().Be(newPhoneNumber);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void ChangePhoneNumber_ShouldThrowEmptyPhoneNumberException_WhenValueIsInvalid(string? invalidPhoneNumber)
        {
            // Arrange
            var user = CreateUser();

            // Act
            Action act = () => user.ChangePhoneNumber(invalidPhoneNumber!);

            // Assert
            act.Should().Throw<EmptyPhoneNumberException>();
        }

        [Fact]
        public void ChangePreferredLanguage_ShouldUpdatePreferredLanguage_WhenValueIsValid()
        {
            // Arrange
            var user = CreateUser();
            var newPreferredLanguage = "es";

            // Act
            user.ChangePreferredLanguage(newPreferredLanguage);

            // Assert
            user.PreferredLanguage.Should().Be(newPreferredLanguage);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void ChangePreferredLanguage_ShouldThrowEmptyPreferredLanguageException_WhenValueIsInvalid(string? invalidPreferredLanguage)
        {
            // Arrange
            var user = CreateUser();

            // Act
            Action act = () => user.ChangePreferredLanguage(invalidPreferredLanguage!);

            // Assert
            act.Should().Throw<EmptyPreferredLanguageException>();
        }

        [Fact]
        public void EnableOnlineStatus_ShouldSetIsOnlineToTrue_WhenUserIsOffline()
        {
            // Arrange
            var user = CreateUser();

            // Act
            user.EnableOnlineStatus();

            // Assert
            user.IsOnline.Should().BeTrue();
        }

        [Fact]
        public void EnableOnlineStatus_ShouldRemainTrue_WhenUserIsAlreadyOnline()
        {
            // Arrange
            var user = CreateUser();
            user.EnableOnlineStatus();

            // Act
            user.EnableOnlineStatus();

            // Assert
            user.IsOnline.Should().BeTrue();
        }

        [Fact]
        public void DisableOnlineStatus_ShouldSetIsOnlineToFalse_WhenUserIsOnline()
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
        public void DisableOnlineStatus_ShouldRemainFalse_WhenUserIsAlreadyOffline()
        {
            // Arrange
            var user = CreateUser();

            // Act
            user.DisableOnlineStatus();

            // Assert
            user.IsOnline.Should().BeFalse();
        }

        [Fact]
        public void SetEmailAsConfirmed_ShouldSetIsEmailConfirmedToTrue()
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
        public void SetLockoutDuration_ShouldUpdateLockOutDuration()
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
        public void SetLockoutEnd_ShouldUpdateLockOutEnd_WhenValueIsInTheFuture()
        {
            // Arrange
            var user = CreateUser();
            var futureLockoutEnd = DateTimeOffset.UtcNow.AddMinutes(5);

            // Act
            user.SetLockoutEnd(futureLockoutEnd);

            // Assert
            user.LockOutEnd.Should().Be(futureLockoutEnd);
        }

        [Fact]
        public void SetLockoutEnd_ShouldThrowExpiredLockOutEndDateException_WhenValueIsInThePast()
        {
            // Arrange
            var user = CreateUser();
            var expiredLockoutEnd = DateTimeOffset.UtcNow.AddSeconds(-1);

            // Act
            Action act = () => user.SetLockoutEnd(expiredLockoutEnd);

            // Assert
            act.Should().Throw<ExpiredLockOutEndDateException>();
        }

        [Fact]
        public void UpdateLastLoginTime_ShouldSetLastLoginAt()
        {
            // Arrange
            var user = CreateUser();

            // Act
            user.UpdateLastLoginTime();

            // Assert
            user.LastLoginAt.Should().NotBeNull();
            user.LastLoginAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }

        [Fact]
        public void UpdatedLastPasswordChangeTime_ShouldSetPasswordChangedAt()
        {
            // Arrange
            var user = CreateUser();

            // Act
            user.UpdatedLastPasswordChangeTime();

            // Assert
            user.PasswordChangedAt.Should().NotBeNull();
            user.PasswordChangedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }

        [Fact]
        public void UpdatedLastProfileUpdateTime_ShouldRefreshUpdatedAt()
        {
            // Arrange
            var user = CreateUser();
            var originalUpdatedAt = user.UpdatedAt;

            // Act
            user.UpdatedLastProfileUpdateTime();

            // Assert
            user.UpdatedAt.Should().BeOnOrAfter(originalUpdatedAt);
            user.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
        }

        private static User CreateUser() =>
            new("Jane", "Doe", "jane.doe@email.com", "hashed-password", "8095551234", "en");
    }
}