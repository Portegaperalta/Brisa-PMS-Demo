
using BrisaPMS.API.Middlewares;
using BrisaPMS.API.Services;
using BrisaPMS.Application;
using BrisaPMS.Application.Contracts.Services;
using BrisaPMS.Identity;
using BrisaPMS.Persistence;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BrisaPMS.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Services
            builder.Services.AddControllers();

            builder.Services.AddOpenApi();

            builder.Services.AddApplicationServices();

            builder.Services.AddPersistenceServices(builder.Configuration);

            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

            builder.Services.AddAuthentication().AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtKey"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("isAdmin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("isManager", policy => policy.RequireRole("Manager"));
                options.AddPolicy("isAccountant", policy => policy.RequireRole("Accountant"));
                options.AddPolicy("isReceptionist", policy => policy.RequireRole("Receptionist"));
                options.AddPolicy("isHouseKeeper", policy => policy.RequireRole("Housekeeper"));
                options.AddPolicy("isCleaningStaff", policy => policy.RequireRole("CleaningStaff"));
                options.AddPolicy("AdminManagerOnly", policy => policy.RequireRole("Admin", "Manager"));
                options.AddPolicy("AdminManagerReceptionistOnly", policy => policy.RequireRole("Admin", "Manager", "Receptionist"));
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // Middlewares
            app.UseExceptionHandlerMiddleware();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            await app.Services.SeedRolesAsync();

            app.Run();
        }
    }
}