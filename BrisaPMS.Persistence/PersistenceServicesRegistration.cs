using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BrisaPMS.Persistence
{
    public static class PersistenceServicesRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<BrisaPmsDbContext>(options =>
                options.UseSqlServer());

            services.AddScoped<IAmenitiesRepository, AmenitiesRepository>();
            
            services.AddScoped<IBookingsRepository, BookingsRepository>();
            
            services.AddScoped<ICompaniesRepository, CompaniesRepository>();
            
            return services;
        }
    }
}