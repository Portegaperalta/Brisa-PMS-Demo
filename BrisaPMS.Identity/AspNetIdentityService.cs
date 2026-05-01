using BrisaPMS.Application.Contracts.Services;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Domain.Users;
using BrisaPMS.Identity.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BrisaPMS.Identity
{
    public class AspNetIdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;

        public AspNetIdentityService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task CreateUserAsync(string email, string password, UserRole role, Guid domainUserId)
        {
            var appUser = new AppUser
            {
                UserName = email,
                Email = email,
                DomainUserId = domainUserId
            };

            var result = await _userManager.CreateAsync(appUser, password);

            if (result.Succeeded is not true)
                throw new IdentityException(result.Errors.Select(e => e.Description));

            await _userManager.AddToRoleAsync(appUser, role.ToString());
        }

        public async Task<bool> CheckPasswordAsync(Guid domainUserId, string password)
        {
            var appUser = await _userManager.Users
                          .FirstOrDefaultAsync(u => u.DomainUserId == domainUserId);

            if (appUser is null)
                return false;

            return await _userManager.CheckPasswordAsync(appUser, password);
        }

        public async Task UpdatePasswordAsync(Guid domainUserId, string newPassword)
        {
            var appUser = await _userManager.Users
                          .FirstOrDefaultAsync(u => u.DomainUserId == domainUserId);

            if (appUser is null)
                throw new UserNotFoundException();

            var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            var result = await _userManager.ResetPasswordAsync(appUser, token, newPassword);

            if (result.Succeeded is not true)
                throw new IdentityException(result.Errors.Select(e => e.Description));
        }

        public async Task UpdateEmailAsync(Guid domainUserId, string newEmail)
        {
            var appUser = await _userManager.Users
                          .FirstOrDefaultAsync(u => u.DomainUserId == domainUserId);

            if (appUser is null)
                throw new NotFoundException("User", domainUserId);

            var token = await _userManager.GenerateChangeEmailTokenAsync(appUser, newEmail);
            var result = await _userManager.ChangeEmailAsync(appUser, newEmail, token);

            await _userManager.SetUserNameAsync(appUser, newEmail);

            if (result.Succeeded is not true)
                throw new IdentityException(result.Errors.Select(e => e.Description));
        }

        public async Task UpdatePhoneNumberAsync(Guid domainUserId, string newPhoneNumber)
        {
            var appUser = await _userManager.Users
                          .FirstOrDefaultAsync(u => u.DomainUserId == domainUserId);

            if (appUser is null)
                throw new NotFoundException("User", domainUserId);

            var token = await _userManager.GenerateChangePhoneNumberTokenAsync(appUser, newPhoneNumber);
            var result = await _userManager.ChangePhoneNumberAsync(appUser, newPhoneNumber, token);

            if (result.Succeeded is not true)
                throw new IdentityException(result.Errors.Select(e => e.Description));
        }

        public async Task<bool> IsEmailUniqueAsync(string email) => await _userManager.Users.AnyAsync(u => u.Email == email);

        public async Task AssignRoleAsync(Guid domainUserId, UserRole role)
        {
            var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.DomainUserId == domainUserId);

            if (appUser is null)
                throw new UserNotFoundException();

            var currentRoles = await _userManager.GetRolesAsync(appUser);

            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(appUser, currentRoles);

            var result = await _userManager.AddToRoleAsync(appUser, role.ToString());

            if (result.Succeeded is not true)
                throw new IdentityException(result.Errors.Select(e => e.Description));
        }
    }
}