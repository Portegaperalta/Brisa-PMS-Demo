using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Domain.Users;

public class User
{
    // Attributes
    public Guid Id { get; init; }
    public UserRole Role { get; private set; }
    public Guid? HotelId { get; init; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; private set; }
    public Password PasswordHash { get; private set; }
    public PhoneNumber? PhoneNumber { get; private set; }
    public UserPreferredLanguage PreferredLanguage { get; private set; }
    public bool IsOnline { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsEmailConfirmed { get; private set; }
    public int FailedLoginAttempts { get; private set; }
    public TimeSpan? LockOutDuration { get; private set; }
    public DateTimeOffset? LockOutEnd { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public DateTime? PasswordChangedAt { get; private set; }

    //Constructor

    public User
    (
        UserRole role,
        Guid? hotelId,
        string firstName,
        string lastName,
        Email email,
        Password password,
        UserPreferredLanguage preferredLanguage,
        PhoneNumber? phoneNumber = null,
        bool isActive = true)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new EmptyRequiredFieldException("First Name");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new EmptyRequiredFieldException("Last Name");

        if (!Enum.IsDefined<UserPreferredLanguage>(preferredLanguage))
            throw new LanguageNotSupportedException();

        Id = Guid.CreateVersion7();
        Role = role;
        HotelId = hotelId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = password;
        PhoneNumber = phoneNumber;
        PreferredLanguage = preferredLanguage;
        IsOnline = false;
        IsActive = isActive;
        IsEmailConfirmed = false;
        FailedLoginAttempts = 0;
        LockOutDuration = null;
        LockOutEnd = null;
        LastLoginAt = null;
        PasswordChangedAt = null;
    }

    // Behavioral Methods
    public void ChangeRole(UserRole newRole) => Role = newRole;

    public void UpdateFirstName(string newFirstName)
    {
        if (string.IsNullOrWhiteSpace(newFirstName))
            throw new EmptyRequiredFieldException("First Name");

        FirstName = newFirstName;
    }

    public void UpdateLastName(string newLastName)
    {
        if (string.IsNullOrWhiteSpace(newLastName))
            throw new EmptyRequiredFieldException("Last Name");

        LastName = newLastName;
    }

    public void ChangeEmail(Email newEmail) => Email = newEmail;

    public void ChangePassword(Password newPassword) => PasswordHash = newPassword;

    public void ChangePhoneNumber(PhoneNumber newPhoneNumber) => PhoneNumber = newPhoneNumber;

    public void UpdatePreferredLanguage(UserPreferredLanguage newPreferredLanguage)
    {
        if (!Enum.IsDefined<UserPreferredLanguage>(newPreferredLanguage))
            throw new LanguageNotSupportedException();

        PreferredLanguage = newPreferredLanguage;
    }

    public void EnableOnlineStatus()
    {
        if (IsOnline is not true)
            IsOnline = true;
    }

    public void DisableOnlineStatus()
    {
        if (IsOnline)
        {
            IsOnline = false;
        }
    }

    public void SetEmailAsConfirmed() => IsEmailConfirmed = true;

    public void IncreaseFailedLoginAttempts() => FailedLoginAttempts++;

    public void SetLockoutDuration(TimeSpan lockoutDuration) => LockOutDuration = lockoutDuration;

    public void SetLockoutEnd(DateTimeOffset lockOutEnd)
    {
        var currentTime = DateTimeOffset.UtcNow;

        if (lockOutEnd < currentTime)
            throw new ExpiredLockOutEndDateException();

        LockOutEnd = lockOutEnd;
    }

    public void UpdateLastLoginTime() => LastLoginAt = DateTime.UtcNow;

    public void UpdatedLastPasswordChangeTime() => PasswordChangedAt = DateTime.UtcNow;
}