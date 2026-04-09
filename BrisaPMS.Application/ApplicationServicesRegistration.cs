using BrisaPMS.Application.UseCases.Hotels.Commands.ActivateHotel;
using BrisaPMS.Application.UseCases.Hotels.Commands.CreateHotel;
using BrisaPMS.Application.UseCases.Hotels.Commands.DeactivateHotel;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelAddressInfo;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelBrandInfo;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelCheckOutPolicy;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelContactInfo;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelDefaultCurrency;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelRates;
using BrisaPMS.Application.UseCases.Hotels.Queries.GetHotelById;
using BrisaPMS.Application.Utilities.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace BrisaPMS.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddTransient<IMediator, SimpleMediator>();
        
        // Hotel commands
        services.AddScoped<IRequestHandler<ActivateHotelCommand, bool>,
                                    ActivateHotelUseCase>();
        
        services.AddScoped<IRequestHandler<CreateHotelCommand, Guid>,
                                    CreateHotelUseCase>();
        
        services.AddScoped<IRequestHandler<DeactivateHotelCommand, bool>,
                                    DeactivateHotelUseCase>();
        
        services.AddScoped<IRequestHandler<UpdateHotelAddressInfoCommand, bool>,
                                    UpdateHotelAddressInfoUseCase>();
        
        services.AddScoped<IRequestHandler<UpdateHotelBrandInfoCommand, bool>, 
                                    UpdateHotelBrandInfoUseCase>();
        
        services.AddScoped<IRequestHandler<UpdateHotelCheckOutPolicyCommand, bool>,
                                    UpdateHotelCheckOutPolicyUseCase>();

        services.AddScoped<IRequestHandler<UpdateHotelContactInfoCommand, bool>, 
                                    UpdateHotelContactInfoUseCase>();

        services.AddScoped<IRequestHandler<UpdateHotelDefaultCurrencyCommand, bool>, 
                                    UpdateHotelDefaultCurrencyUseCase>();

        services.AddScoped<IRequestHandler<UpdateHotelRatesCommand, bool>,
                                     UpdateHotelRatesUseCase>();
        
        // Hotel queries
        services.AddScoped<IRequestHandler<GetHotelByIdQuery, HotelDto>, GetHotelByIdUseCase>();
        
        return services;
    }
}