using BrisaPMS.Domain.Exceptions;

namespace BrisaPMS.Domain.Entities;

public class User
{
    // Attributes
    public required Guid Id { get; init; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash {get; set;}
    public string? PhoneNumber { get; set; }
    public required string PreferredLanguage { get; set; }
    public required bool IsOnline { get; set; }
    public required bool IsActive { get; set; }
    public required bool IsEmailConfirmed { get; set; }
    public required int FailedLoginAttempts { get; set; }
    public DateTime? LockoutExpiresAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime? PasswordChangedAt { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    
    //Constructor

    public User(string firstName, string lastName, string email, 
        string passwordHash, string? phoneNumber, string preferredLanguage,
        bool isActive = true)
    {
        if (string.IsNullOrWhiteSpace(firstName) is true)
            throw new EmptyFirstNameException();
        
        if (string.IsNullOrWhiteSpace(lastName) is true )
            throw new EmptyLastNameException();

        if (string.IsNullOrWhiteSpace(email) is true)
            throw new EmptyEmailException();

        if (string.IsNullOrWhiteSpace(passwordHash) is true)
            throw new EmptyPasswordHashException();
        
        if (string.IsNullOrWhiteSpace(preferredLanguage) == true)
            throw new EmptyPreferredLanguageException();
        
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
        LockoutExpiresAt = null;
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
        if(string.IsNullOrWhiteSpace(newLastName) is true)
            throw new EmptyLastNameException();
        
        LastName = newLastName;
    }

    public void ChangeEmail(string newEmail)
    {
        if(string.IsNullOrWhiteSpace(newEmail) is true)
            throw new EmptyEmailException();
        
        Email = newEmail;
    }

    public void ChangePasswordHash(string newPasswordHash)
    {
        if(string.IsNullOrWhiteSpace(newPasswordHash) is true)
            throw new EmptyPasswordHashException();
        
        PasswordHash = newPasswordHash;
    }

    public void ChangePhoneNumber(string newPhoneNumber) 
        => PhoneNumber =  newPhoneNumber;

    public void ChangePreferredLanguage(string newPreferredLanguage)
    {
        if (string.IsNullOrWhiteSpace(newPreferredLanguage) is true)
            throw new EmptyPreferredLanguageException();
        
        PreferredLanguage = newPreferredLanguage;
    }
}