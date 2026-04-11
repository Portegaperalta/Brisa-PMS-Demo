using BrisaPMS.Application.UseCases.Hotels.Commands.ActivateHotel;
using BrisaPMS.Application.UseCases.Hotels.Commands.CreateHotel;
using BrisaPMS.Application.UseCases.Hotels.Commands.DeactivateHotel;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelAddressInfo;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelBrandInfo;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelCheckOutPolicy;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelContactInfo;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelDefaultCurrency;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelRates;
using BrisaPMS.Application.UseCases.Hotels.Queries.GetAllHotels;
using BrisaPMS.Application.UseCases.Hotels.Queries.GetHotelById;
using BrisaPMS.Application.UseCases.Hotels.Shared;
using BrisaPMS.Application.UseCases.Rooms.Commands.ChangeRoomType;
using BrisaPMS.Application.UseCases.Rooms.Commands.CreateRoom;
using BrisaPMS.Application.UseCases.Rooms.Commands.SetAsPendingRestock;
using BrisaPMS.Application.UseCases.Rooms.Commands.SetAsRestocked;
using BrisaPMS.Application.UseCases.Rooms.Commands.UpdateAvailabilityStatus;
using BrisaPMS.Application.UseCases.Rooms.Commands.UpdateHygieneStatus;
using BrisaPMS.Application.UseCases.Rooms.Commands.UpdateRoomNumber;
using BrisaPMS.Application.UseCases.RoomTypes.Commands.CreateRoomType;
using BrisaPMS.Application.Utilities.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace BrisaPMS.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddTransient<IMediator, SimpleMediator>();
        
        // Hotels services
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
        
        services.AddScoped<IRequestHandler<GetHotelByIdQuery, HotelDto>, GetHotelByIdUseCase>();
        
        services.AddScoped<IRequestHandler<GetAllHotelsQuery, List<HotelDto>>,
                                    GetAllHotelsUseCase>();
        
        // Room types services
        services.AddScoped<IRequestHandler<CreateRoomTypeCommand, Guid>, CreateRoomTypeUseCase>();
        
        // Rooms services
        services.AddScoped<IRequestHandler<ChangeRoomTypeCommand, bool>, 
                                    ChangeRoomTypeUseCase>();
        
        services.AddScoped<IRequestHandler<CreateRoomCommand, Guid>, CreateRoomUseCase>();
        
        services.AddScoped<IRequestHandler<SetAsPendingRestockCommand, bool>, 
                                      SetAsPendingRestockUseCase>();
        
        services.AddScoped<IRequestHandler<SetAsRestockedCommand, bool>,
                                     SetAsRestockedUseCase>();
        
        services.AddScoped<IRequestHandler<UpdateAvailabilityStatusCommand, bool>,
                                    UpdateAvailabilityStatusUseCase>();
        
        services.AddScoped<IRequestHandler<UpdateHygieneStatusCommand, bool>,
                                    UpdateHygieneStatusUseCase>();

        services.AddScoped<IRequestHandler<UpdateRoomNumberCommand, bool>,
                                    UpdateRoomNumberUseCase>();
        
        return services;
    }
}