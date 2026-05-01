using BrisaPMS.Domain.Users;

namespace BrisaPMS.Application.Contracts.Services
{
    public interface IIdentityService
    {
        Task CreateUserAsync(string email, string password, UserRole role, Guid domainUserId);
        Task<bool> CheckPasswordAsync(Guid domainUserId, string password);
        Task UpdatePasswordAsync(Guid domainUserId, string newPassword);
        Task UpdateEmailAsync(Guid domainUserId, string newEmail);
        Task<bool> IsEmailUniqueAsync(string email);
        Task AssignRoleAsync(Guid domainUserId, UserRole role);
    }
}