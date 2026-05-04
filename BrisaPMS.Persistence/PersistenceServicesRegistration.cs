using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Persistence.Repositories;
using BrisaPMS.Persistence.UnitsOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BrisaPMS.Persistence
{
    public static class PersistenceServicesRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<BrisaPmsDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), 
                b => b.MigrationsAssembly("BrisaPMS.Persistence")));

            services.AddScoped<IUnitOfWork, UnitOfWorkEfCore>();

            services.AddScoped<IAmenitiesRepository, AmenitiesRepository>();
            
            services.AddScoped<IBookingsRepository, BookingsRepository>();
            
            services.AddScoped<ICompaniesRepository, CompaniesRepository>();
            
            services.AddScoped<IGuestsRepository, GuestsRepository>();
            
            services.AddScoped<IHotelsRepository, HotelsRepository>();
            
            services.AddScoped<IHouseKeepingTasksRepository, HouseKeepingTasksRepository>();
            
            services.AddScoped<IRoomsRepository, RoomsRepository>();

            services.AddScoped<IRoomTypesRepository, RoomTypesRepository>();
            
            services.AddScoped<IStaysRepository, StaysRepository>();
            
            services.AddScoped<IUsersRepository, UsersRepository>();
            
            return services;
        }
    }
}