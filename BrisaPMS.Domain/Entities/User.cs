using BrisaPMS.Domain.Exceptions;

namespace BrisaPMS.Domain.Entities;

public class User
{
    // Attributes
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash {get; private set; }
    public string? PhoneNumber { get; private set; }
    public string PreferredLanguage { get; private set; }
    public bool IsOnline { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsEmailConfirmed { get; private set; }
    public int FailedLoginAttempts { get; private set; }
    public DateTime? LockoutExpiresAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public DateTime? PasswordChangedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
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