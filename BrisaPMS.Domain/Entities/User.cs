using System;
using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;
using BrisaPMS.Domain.ValueObjects;

namespace BrisaPMS.Domain.Entities;

public class User
{
    // Attributes
    public Guid Id { get; private init; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string? PhoneNumber { get; private set; }
    public PreferredLanguage PreferredLanguage { get; private set; }
    public bool IsOnline { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsEmailConfirmed { get; private set; }
    public int FailedLoginAttempts { get; private set; }
    public TimeSpan? LockOutDuration { get; private set; }
    public DateTimeOffset? LockOutEnd { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public DateTime? PasswordChangedAt { get; private set; }
    public DateTime CreatedAt { get; private init; }
    public DateTime? UpdatedAt { get; private set; }

    private readonly int FirstNameMaxLength = 250;
    private readonly int LastNameMaxLength = 250;

    //Constructor

    public User(string firstName, 
        string lastName, 
        Email email,
        string passwordHash,
        string? phoneNumber,
        PreferredLanguage preferredLanguage,
        bool isActive = true)
    {
        if (string.IsNullOrWhiteSpace(firstName) is true)
            throw new EmptyFirstNameException();

        if (string.IsNullOrWhiteSpace(lastName) is true)
            throw new EmptyLastNameException();

        if (firstName.Length > FirstNameMaxLength)
            throw new MaxCharacterLimitException(FirstNameMaxLength, "First name");

        if (lastName.Length > LastNameMaxLength)
            throw new MaxCharacterLimitException(LastNameMaxLength, "Last name");

        if (string.IsNullOrWhiteSpace(passwordHash) is true)
            throw new EmptyPasswordHashException();

        if (!Enum.IsDefined<PreferredLanguage>(preferredLanguage))
            throw new LanguageNotSupportedException();

        Id = Guid.CreateVersion7();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
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
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    // Behavioral Methods
    public void ChangeFirstName(string newFirstName)
    {
        if (string.IsNullOrWhiteSpace(newFirstName) is true)
            throw new EmptyFirstNameException();

        if (newFirstName.Length > FirstNameMaxLength)
            throw new MaxCharacterLimitException(FirstNameMaxLength, "First name");

        FirstName = newFirstName;
    }

    public void ChangeLastName(string newLastName)
    {
        if (string.IsNullOrWhiteSpace(newLastName) is true)
            throw new EmptyLastNameException();

        if (newLastName.Length > LastNameMaxLength)
            throw new MaxCharacterLimitException(LastNameMaxLength, "Last name");

        LastName = newLastName;
    }

    public void ChangeEmail(Email newEmail) => Email = newEmail;

    public void ChangePasswordHash(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash) is true)
            throw new EmptyPasswordHashException();

        PasswordHash = newPasswordHash;
    }

    public void ChangePhoneNumber(string newPhoneNumber)
    {
        if (string.IsNullOrWhiteSpace(newPhoneNumber) is true)
            throw new EmptyPhoneNumberException();

        PhoneNumber = newPhoneNumber;
    }

    public void ChangePreferredLanguage(PreferredLanguage newPreferredLanguage)
    {
        if (!Enum.IsDefined<PreferredLanguage>(newPreferredLanguage))
            throw new LanguageNotSupportedException();
        
        PreferredLanguage = newPreferredLanguage;
    }

    public void EnableOnlineStatus()
    {
        if (IsOnline is false)
            IsOnline = true;
    }

    public void DisableOnlineStatus()
    {
        if (IsOnline is true)
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

    public void UpdatedLastProfileUpdateTime() => UpdatedAt = DateTime.UtcNow;
}