using BrisaPMS.Application.UseCases.Companies;
using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyAddressInfo;
using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyBrandInfo;
using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyContactInfo;
using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyRnc;
using BrisaPMS.Application.UseCases.Companies.Queries.GetCompanyInfo;
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
using BrisaPMS.Application.UseCases.Rooms.Queries.GetAllRooms;
using BrisaPMS.Application.UseCases.Rooms.Queries.GetAllRoomsByHotelId;
using BrisaPMS.Application.UseCases.Rooms.Queries.GetRoomById;
using BrisaPMS.Application.UseCases.Rooms.Shared;
using BrisaPMS.Application.UseCases.RoomTypes.Commands.CreateRoomType;
using BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeBaseRate;
using BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeBedsInfo;
using BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeGeneralInfo;
using BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeOccupancyPolicy;
using BrisaPMS.Application.UseCases.RoomTypes.Queries.GetAllRoomTypes;
using BrisaPMS.Application.UseCases.RoomTypes.Queries.GetRoomTypeById;
using BrisaPMS.Application.UseCases.RoomTypes.Shared;
using BrisaPMS.Application.Utilities.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace BrisaPMS.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddTransient<IMediator, SimpleMediator>();
        
        // Companies services
        services.AddScoped<IRequestHandler<UpdateCompanyAddressInfoCommand, bool>,
                                    UpdateCompanyAddressInfoUseCase>();

        services.AddScoped<IRequestHandler<UpdateCompanyBrandInfoCommand, bool>,
                                UpdateCompanyBrandInfoUseCase>();

        services.AddScoped<IRequestHandler<UpdateCompanyContactInfoCommand, bool>,
                                UpdateCompanyContactInfoUseCase>();

        services.AddScoped<IRequestHandler<UpdateCompanyRncCommand, bool>,
                                    UpdateCompanyRncUseCase>();

        services.AddScoped<IRequestHandler<GetCompanyInfoQuery, CompanyDto>, 
                                    GetCompanyInfoUseCase>();
        
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
        services.AddScoped<IRequestHandler<CreateRoomTypeCommand, Guid>,
                                    CreateRoomTypeUseCase>();

        services.AddScoped<IRequestHandler<UpdateRoomTypeBaseRateCommand, bool>,
                                    UpdateRoomTypeBaseRateUseCase>();
        
        services.AddScoped<IRequestHandler<UpdateRoomTypeBedsInfoCommand, bool>, 
                                    UpdateRoomTypeBedsInfoUseCase>();
        
        services.AddScoped<IRequestHandler<UpdateRoomTypeGeneralInfoCommand, bool>,
                                    UpdateRoomTypeGeneralInfoUseCase>();

        services.AddScoped<IRequestHandler<UpdateRoomTypeOccupancyPolicyCommand, bool>,
                                    UpdateRoomTypeOccupancyPolicyUseCase>();

        services.AddScoped<IRequestHandler<GetAllRoomTypesQuery, List<RoomTypeDto>>,
                                     GetAllRoomTypesUseCase>();

        services.AddScoped<IRequestHandler<GetRoomTypeByIdQuery, RoomTypeDto>,
                                    GetRoomTypeByIdUseCase>();
        
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

        services.AddScoped<IRequestHandler<GetAllRoomsQuery, List<RoomDto>>,
                                    GetAllRoomsUseCase>();
        
        services.AddScoped<IRequestHandler<GetAllRoomsByHotelIdQuery, List<RoomDto>>,
                                    GetAllRoomsByHotelIdUseCase>();
        
        services.AddScoped<IRequestHandler<GetRoomByIdQuery, RoomDto>,
                                     GetRoomByIdUseCase>();
        
        return services;
    }
}