using BrisaPMS.Domain.Users;

namespace BrisaPMS.Application.UseCases.Users.Shared;

public static class MapperExtension
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Role = user.Role.ToString(),
            HotelId = user.HotelId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email.Value,
            PhoneNumber = user.PhoneNumber.Value,
            PreferredLanguage = user.PreferredLanguage.ToString()
        };
    }
}