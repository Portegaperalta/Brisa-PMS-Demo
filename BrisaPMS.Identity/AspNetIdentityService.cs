using BrisaPMS.Application.Contracts.Services;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Domain.Users;
using BrisaPMS.Identity.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BrisaPMS.Identity
{
    public class AspNetIdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AspNetIdentityService(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var appUser = await _userManager.FindByEmailAsync(email);

            if (appUser is null)
                throw new IdentityException($"User with email: {email} not found");

            var isPasswordValid = await _userManager.CheckPasswordAsync(appUser, password);

            if (isPasswordValid is not true)
                throw new InvalidOperationException("Invalid Credentials");

            return await CreateToken(appUser.DomainUserId, email);
        }

        public async Task<string> CreateUserAsync(string email, string password, UserRole role, Guid domainUserId)
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
                throw new NotFoundException("User", domainUserId);

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
                throw new NotFoundException("User", domainUserId);

            var currentRoles = await _userManager.GetRolesAsync(appUser);

            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(appUser, currentRoles);

            var result = await _userManager.AddToRoleAsync(appUser, role.ToString());

            if (result.Succeeded is not true)
                throw new IdentityException(result.Errors.Select(e => e.Description));
        }

        private async Task<string> CreateToken(Guid userId,string email)
        {
            var appUser = await _userManager.FindByEmailAsync(email) ?? throw new NotFoundException("User", userId);

            var userRoles = await _userManager.GetRolesAsync(appUser);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, userRoles.FirstOrDefault() ?? string.Empty)
            };

            var claimsDb = await _userManager.GetClaimsAsync(appUser);

            claims.AddRange(claimsDb);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null,
                                claims: claims, expires: expiration, signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }
    }
}