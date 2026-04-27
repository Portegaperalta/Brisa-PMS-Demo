using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Persistence.Repositories;
using BrisaPMS.Persistence.UnitsOfWork;
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

            services.AddScoped<IUnitOfWork, UnitOfWorkEfCore>();

            services.AddScoped<IAmenitiesRepository, AmenitiesRepository>();
            
            services.AddScoped<IBookingsRepository, BookingsRepository>();
            
            services.AddScoped<ICompaniesRepository, CompaniesRepository>();
            
            services.AddScoped<IGuestsRepository, GuestsRepository>();
            
            services.AddScoped<IHotelsRepository, HotelsRepository>();
            
            services.AddScoped<IHouseKeepingTasksRepository, HouseKeepingTasksRepository>();
            
            services.AddScoped<IRoomsRepository, RoomsRepository>();
            
            services.AddScoped<IStaysRepository, StaysRepository>();
            
            return services;
        }
    }
}