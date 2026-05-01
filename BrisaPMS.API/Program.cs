
using BrisaPMS.API.Services;
using BrisaPMS.Application;
using BrisaPMS.Application.Contracts.Services;
using BrisaPMS.Identity;
using BrisaPMS.Persistence;

namespace BrisaPMS.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Services Area
            builder.Services.AddControllers();

            builder.Services.AddOpenApi();

            builder.Services.AddApplicationServices();

            builder.Services.AddPersistenceServices(builder.Configuration);

            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            await app.Services.SeedRolesAsync();

            app.Run();
        }
    }
}
