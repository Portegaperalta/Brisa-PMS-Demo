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
    public bool IsActive { get; private set; }
    public bool IsEmailConfirmed { get; private set; }
    public int FailedLoginAttempts { get; private set; }
    public TimeSpan? LockOutDuration { get; private set; }
    public DateTimeOffset? LockOutEnd { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public DateTime? PasswordChangedAt { get; private set; }
    
    private User() {}
    
    // Nested Builder
    public class Builder
    {
        private readonly UserRole _role;
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly Email _email;
        private readonly Password _passwordHash;
        private readonly UserPreferredLanguage _preferredLanguage;
        
        private Guid? _hotelId;
        private PhoneNumber? _phoneNumber;

        public Builder
        (
            UserRole role,
            string firstName,
            string lastName,
            Email email,
            Password passwordHash,
            UserPreferredLanguage preferredLanguage
        )
        {
            if (string.IsNullOrWhiteSpace(firstName))  
                throw new EmptyRequiredFieldException("First Name");  
  
            if (string.IsNullOrWhiteSpace(lastName))  
                throw new EmptyRequiredFieldException("Last Name");  
  
            if (!Enum.IsDefined<UserPreferredLanguage>(preferredLanguage))  
                throw new LanguageNotSupportedException(); 
            
            _role = role;
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _passwordHash = passwordHash;
            _preferredLanguage = preferredLanguage;
        }
        
        public Builder WithHotelId(Guid hotelId) { _hotelId = hotelId; return this; }
        public Builder WithPhoneNumber(PhoneNumber phoneNumber) { _phoneNumber = phoneNumber; return this; }

        public User Build()
        {
            return new User()
            {
                Id = Guid.CreateVersion7(),
                Role = _role,
                HotelId = _hotelId,
                FirstName = _firstName,
                LastName = _lastName,
                Email = _email,
                PasswordHash = _passwordHash,
                PhoneNumber = _phoneNumber,
                PreferredLanguage = _preferredLanguage
            };
        }
    }

    // Behavioral Methods
    public void ChangeRole(UserRole newRole)
    {
        if (Role is UserRole.Admin)
            throw new BusinessRuleException("The role of a user with the 'Admin' role cannot be changed.");
        
        Role = newRole;
    }

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