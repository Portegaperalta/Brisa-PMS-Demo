using System;
using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;

namespace BrisaPMS.Domain.Entities;

public class User
{
    // Attributes
    public Guid Id { get; private init; } = Guid.CreateVersion7();
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
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
    public DateTime UpdatedAt { get; private set; }

    //Constructor

    public User(string firstName, 
        string lastName, 
        string email,
        string passwordHash,
        string? phoneNumber,
        PreferredLanguage preferredLanguage,
        bool isActive = true)
    {
        if (string.IsNullOrWhiteSpace(firstName) is true)
            throw new EmptyFirstNameException();

        if (string.IsNullOrWhiteSpace(lastName) is true)
            throw new EmptyLastNameException();

        if (string.IsNullOrWhiteSpace(email) is true)
            throw new EmptyEmailException();

        if (string.IsNullOrWhiteSpace(passwordHash) is true)
            throw new EmptyPasswordHashException();

        if (!Enum.IsDefined<PreferredLanguage>(preferredLanguage))
            throw new LanguageNotSupportedException();

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
        UpdatedAt = DateTime.UtcNow;
    }

    // Behavioral Methods
    public void ChangeFirstName(string newFirstName)
    {
        if (string.IsNullOrWhiteSpace(newFirstName) is true)
            throw new EmptyFirstNameException();

        FirstName = newFirstName;
    }

    public void ChangeLastName(string newLastName)
    {
        if (string.IsNullOrWhiteSpace(newLastName) is true)
            throw new EmptyLastNameException();

        LastName = newLastName;
    }

    public void ChangeEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail) is true)
            throw new EmptyEmailException();

        Email = newEmail;
    }

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