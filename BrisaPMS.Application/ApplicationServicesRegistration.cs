using BrisaPMS.Application.UseCases.Amenities.Commands.ActivateAmenity;
using BrisaPMS.Application.UseCases.Amenities.Commands.CreateAmenity;
using BrisaPMS.Application.UseCases.Amenities.Commands.DeactivateAmenity;
using BrisaPMS.Application.UseCases.Amenities.Commands.DeleteAmenity;
using BrisaPMS.Application.UseCases.Amenities.Commands.UpdateAmenityDetails;
using BrisaPMS.Application.UseCases.Amenities.Queries.GetAllAmenities;
using BrisaPMS.Application.UseCases.Amenities.Queries.GetAmenityById;
using BrisaPMS.Application.UseCases.Amenities.Shared;
using BrisaPMS.Application.UseCases.Bookings.Commands.CancelBooking;
using BrisaPMS.Application.UseCases.Bookings.Commands.ChangeAssignedRoom;
using BrisaPMS.Application.UseCases.Bookings.Commands.ChangeBookingSource;
using BrisaPMS.Application.UseCases.Bookings.Commands.ConfirmBooking;
using BrisaPMS.Application.UseCases.Bookings.Commands.CreateBooking;
using BrisaPMS.Application.UseCases.Bookings.Commands.DeleteBooking;
using BrisaPMS.Application.UseCases.Bookings.Commands.MarkAsNoShow;
using BrisaPMS.Application.UseCases.Bookings.Commands.UpdateCancellationReason;
using BrisaPMS.Application.UseCases.Bookings.Commands.UpdateCheckInOutTimes;
using BrisaPMS.Application.UseCases.Bookings.Commands.UpdateGuestCount;
using BrisaPMS.Application.UseCases.Bookings.Commands.UpdateSpecialRequests;
using BrisaPMS.Application.UseCases.Bookings.Commands.UpdateTotalPrice;
using BrisaPMS.Application.UseCases.Bookings.Queries.GetAllBookings;
using BrisaPMS.Application.UseCases.Bookings.Queries.GetBookingById;
using BrisaPMS.Application.UseCases.Bookings.Queries.GetBookingsByHotelId;
using BrisaPMS.Application.UseCases.Bookings.Queries.Shared;
using BrisaPMS.Application.UseCases.Companies;
using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyAddressInfo;
using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyBrandInfo;
using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyContactInfo;
using BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyRnc;
using BrisaPMS.Application.UseCases.Companies.Queries.GetCompanyInfo;
using BrisaPMS.Application.UseCases.Guests.Commands.BlacklistGuest;
using BrisaPMS.Application.UseCases.Guests.Commands.CreateGuest;
using BrisaPMS.Application.UseCases.Guests.Commands.DeleteGuest;
using BrisaPMS.Application.UseCases.Guests.Commands.MakeGuestVip;
using BrisaPMS.Application.UseCases.Guests.Commands.RevokeGuestVip;
using BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestContactInfo;
using BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestDocumentation;
using BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestGeneralInfo;
using BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestRnc;
using BrisaPMS.Application.UseCases.Guests.Commands.WhitelistGuest;
using BrisaPMS.Application.UseCases.Guests.Queries.GetAllGuestsByHotelId;
using BrisaPMS.Application.UseCases.Guests.Queries.GetGuestById;
using BrisaPMS.Application.UseCases.Guests.Shared;
using BrisaPMS.Application.UseCases.Hotels.Commands.ActivateHotel;
using BrisaPMS.Application.UseCases.Hotels.Commands.CreateHotel;
using BrisaPMS.Application.UseCases.Hotels.Commands.DeactivateHotel;
using BrisaPMS.Application.UseCases.Hotels.Commands.DeleteHotel;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelAddressInfo;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelBrandInfo;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelCheckOutPolicy;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelContactInfo;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelDefaultCurrency;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelRates;
using BrisaPMS.Application.UseCases.Hotels.Queries.GetAllHotels;
using BrisaPMS.Application.UseCases.Hotels.Queries.GetHotelById;
using BrisaPMS.Application.UseCases.Hotels.Shared;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.CancelHouseKeepingTask;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.ChangeHouseKeepingTaskType;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.ChangeTaskDeadline;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.CompleteHouseKeepingTask;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.CreateHouseKeepingTask;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.DeleteHouseKeepingTask;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.ReportIncident;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.ReassignHouseKeepingTask;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.StartHouseKeepingTask;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.UpdateHouseKeepingTaskNotes;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.UpdateHouseKeepingTaskPriority;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.UpdateIncidentDescription;
using BrisaPMS.Application.UseCases.HouseKeeping.Queries.GetAllHouseKeepingTasks;
using BrisaPMS.Application.UseCases.HouseKeeping.Queries.GetAllHouseKeepingTasksByHotelId;
using BrisaPMS.Application.UseCases.HouseKeeping.Queries.GetAllHouseKeepingTasksByRoomId;
using BrisaPMS.Application.UseCases.HouseKeeping.Queries.GetHouseKeepingTaskById;
using BrisaPMS.Application.UseCases.HouseKeeping.Shared;
using BrisaPMS.Application.UseCases.Rooms.Commands.ChangeRoomType;
using BrisaPMS.Application.UseCases.Rooms.Commands.CreateRoom;
using BrisaPMS.Application.UseCases.Rooms.Commands.DeleteRoom;
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
using BrisaPMS.Application.UseCases.RoomTypes.Commands.DeleteRoomType;
using BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeBaseRate;
using BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeBedsInfo;
using BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeGeneralInfo;
using BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeOccupancyPolicy;
using BrisaPMS.Application.UseCases.RoomTypes.Queries.GetAllRoomTypes;
using BrisaPMS.Application.UseCases.RoomTypes.Queries.GetRoomTypeById;
using BrisaPMS.Application.UseCases.RoomTypes.Shared;
using BrisaPMS.Application.UseCases.Stays.Commands.CompleteStay;
using BrisaPMS.Application.UseCases.Stays.Commands.CreateStay;
using BrisaPMS.Application.UseCases.Stays.Commands.DeleteStay;
using BrisaPMS.Application.UseCases.Stays.Commands.IncreaseNightCount;
using BrisaPMS.Application.UseCases.Stays.Queries.GetAllStays;
using BrisaPMS.Application.UseCases.Stays.Queries.GetAllStaysByGuestId;
using BrisaPMS.Application.UseCases.Stays.Queries.GetAllStaysByHotelId;
using BrisaPMS.Application.UseCases.Stays.Queries.GetStayById;
using BrisaPMS.Application.UseCases.Stays.Shared;
using BrisaPMS.Application.UseCases.Users.Commands.ChangeEmail;
using BrisaPMS.Application.UseCases.Users.Commands.ChangePassword;
using BrisaPMS.Application.UseCases.Users.Commands.ChangePhoneNumber;
using BrisaPMS.Application.UseCases.Users.Commands.ChangePreferredLanguage;
using BrisaPMS.Application.UseCases.Users.Commands.ChangeRole;
using BrisaPMS.Application.UseCases.Users.Commands.CreateUser;
using BrisaPMS.Application.UseCases.Users.Commands.Login;
using BrisaPMS.Application.UseCases.Users.Commands.UpdateUserName;
using BrisaPMS.Application.UseCases.Users.Queries.GetAllUsers;
using BrisaPMS.Application.UseCases.Users.Queries.GetAllUsersByHotelId;
using BrisaPMS.Application.UseCases.Users.Queries.GetUserById;
using BrisaPMS.Application.UseCases.Users.Shared;
using BrisaPMS.Application.Utilities.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace BrisaPMS.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddTransient<IMediator, SimpleMediator>();

        // Amenities services
        services.AddScoped<IRequestHandler<ActivateAmenityCommand, bool>,
                                    ActivateAmenityUseCase>();

        services.AddScoped<IRequestHandler<CreateAmenityCommand, AmenityDto>,
                                    CreateAmenityUseCase>();

        services.AddScoped<IRequestHandler<DeactivateAmenityCommand, bool>,
                                    DeactivateAmenityUseCase>();

        services.AddScoped<IRequestHandler<UpdateAmenityDetailsCommand, bool>,
                                    UpdateAmenityDetailsUseCase>();

        services.AddScoped<IRequestHandler<DeleteAmenityCommand, bool>,
                                    DeleteAmenityUseCase>();

        services.AddScoped<IRequestHandler<GetAllAmenitiesQuery, List<AmenityDto>>,
                                    GetAllAmenitiesUseCase>();

        services.AddScoped<IRequestHandler<GetAmenityByIdQuery, AmenityDto>,
                                     GetAmenityByIdUseCase>();

        // Bookings services
        services.AddScoped<IRequestHandler<CancelBookingCommand, bool>,
                                     CancelBookingUseCase>();

        services.AddScoped<IRequestHandler<ChangeAssignedRoomCommand, bool>,
                                    ChangeAssignedRoomUseCase>();

        services.AddScoped<IRequestHandler<ChangeBookingSourceCommand, bool>,
                                    ChangeBookingSourceUseCase>();

        services.AddScoped<IRequestHandler<ConfirmBookingCommand, bool>,
                                    ConfirmBookingUseCase>();

        services.AddScoped<IRequestHandler<CreateBookingCommand, BookingDto>,
                                    CreateBookingUseCase>();

        services.AddScoped<IRequestHandler<MarkAsNoShowCommand, bool>,
                                    MarkAsNoShowUseCase>();

        services.AddScoped<IRequestHandler<UpdateCancellationReasonCommand, bool>,
                                    UpdateCancellationReasonUseCase>();

        services.AddScoped<IRequestHandler<UpdateCheckInOutTimesCommand, bool>,
                                    UpdateCheckInOutTimesUseCase>();

        services.AddScoped<IRequestHandler<UpdateGuestCountCommand, bool>,
                                    UpdateGuestCountUseCase>();

        services.AddScoped<IRequestHandler<UpdateSpecialRequestsCommand, bool>,
                                    UpdateSpecialRequestsUseCase>();

        services.AddScoped<IRequestHandler<UpdateTotalPriceCommand, bool>,
                                    UpdateTotalPriceUseCase>();

        services.AddScoped<IRequestHandler<DeleteBookingCommand, bool>,
                                    DeleteBookingUseCase>();

        services.AddScoped<IRequestHandler<GetAllBookingsQuery, List<BookingDto>>,
                                    GetAllBookingsUseCase>();

        services.AddScoped<IRequestHandler<GetBookingByIdQuery, BookingDto>,
                                    GetBookingByIdUseCase>();

        services.AddScoped<IRequestHandler<GetBookingsByHotelIdQuery, List<BookingDto>>,
                                    GetBookingsByHotelIdUseCase>();

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

        // Guests services
        services.AddScoped<IRequestHandler<BlacklistGuestCommand, bool>,
                                    BlacklistGuestUseCase>();

        services.AddScoped<IRequestHandler<CreateGuestCommand, GuestDto>,
                                    CreateGuestUseCase>();

        services.AddScoped<IRequestHandler<UpdateGuestGeneralInfoCommand, bool>,
                                    UpdateGuestGeneralInfoUseCase>();

        services.AddScoped<IRequestHandler<UpdateGuestContactInfoCommand, bool>,
                                    UpdateGuestContactInfoUseCase>();

        services.AddScoped<IRequestHandler<UpdateGuestDocumentationCommand, bool>,
                                    UpdateGuestDocumentationUseCase>();

        services.AddScoped<IRequestHandler<UpdateGuestRncCommand, bool>,
                                    UpdateGuestRncUseCase>();

        services.AddScoped<IRequestHandler<MakeGuestVipCommand, bool>,
                                    MakeGuestVipUseCase>();

        services.AddScoped<IRequestHandler<RevokeGuestVipCommand, bool>,
                                    RevokeGuestVipUseCase>();

        services.AddScoped<IRequestHandler<WhitelistGuestCommand, bool>,
                                    WhitelistGuestUseCase>();
        
        services.AddScoped<IRequestHandler<DeleteGuestCommand, bool>, 
                                    DeleteGuestUseCase>();

        services.AddScoped<IRequestHandler<GetGuestByIdQuery, GuestDto>,
                                    GetGuestByIdUseCase>();

        services.AddScoped<IRequestHandler<GetAllGuestsByHotelIdQuery, List<GuestDto>>,
                                    GetAllGuestsByHotelIdUseCase>();

        // Hotels services
        services.AddScoped<IRequestHandler<ActivateHotelCommand, bool>,
                                    ActivateHotelUseCase>();

        services.AddScoped<IRequestHandler<CreateHotelCommand, HotelDto>,
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
        
        services.AddScoped<IRequestHandler<DeleteHotelCommand, bool>, 
                                    DeleteHotelUseCase>();

        services.AddScoped<IRequestHandler<GetHotelByIdQuery, HotelDto>, GetHotelByIdUseCase>();

        services.AddScoped<IRequestHandler<GetAllHotelsQuery, List<HotelDto>>,
                                    GetAllHotelsUseCase>();

        // HouseKeeping services
        services.AddScoped<IRequestHandler<CancelHouseKeepingTaskCommand, bool>,
                                    CancelHouseKeepingTaskUseCase>();

        services.AddScoped<IRequestHandler<ChangeHouseKeepingTaskTypeCommand, bool>,
                                    ChangeHouseKeepingTaskTypeUseCase>();

        services.AddScoped<IRequestHandler<ChangeTaskDeadlineCommand, bool>,
                                    ChangeTaskDeadlineUseCase>();

        services.AddScoped<IRequestHandler<CompleteHouseKeepingTaskCommand, bool>,
                                    CompleteHouseKeepingTaskUseCase>();

        services.AddScoped<IRequestHandler<CreateHouseKeepingTaskCommand, Guid>,
                                    CreateHouseKeepingTaskUseCase>();

        services.AddScoped<IRequestHandler<ReportIncidentCommand, bool>,
                                    ReportIncidentUseCase>();

        services.AddScoped<IRequestHandler<ReassignHouseKeepingTaskCommand, bool>,
                                    ReassignHouseKeepingTaskUseCase>();

        services.AddScoped<IRequestHandler<StartHouseKeepingTaskCommand, bool>,
                                    StartHouseKeepingTaskUseCase>();

        services.AddScoped<IRequestHandler<UpdateHouseKeepingTaskNotesCommand, bool>,
                                    UpdateHouseKeepingTaskNotesUseCase>();

        services.AddScoped<IRequestHandler<UpdateHouseKeepingTaskPriorityCommand, bool>,
                                    UpdateHouseKeepingTaskPriorityUseCase>();

        services.AddScoped<IRequestHandler<UpdateIncidentDescriptionCommand, bool>,
                                    UpdateIncidentDescriptionUseCase>();
        
        services.AddScoped<IRequestHandler<DeleteHouseKeepingTaskCommand, bool>,
                                    DeleteHouseKeepingTaskUseCase>();

        services.AddScoped<IRequestHandler<GetAllHouseKeepingTasksQuery, List<HouseKeepingTaskDto>>,
                                    GetAllHouseKeepingTasksUseCase>();

        services.AddScoped<IRequestHandler<GetAllHouseKeepingTasksByHotelIdQuery, List<HouseKeepingTaskDto>>,
                                    GetAllHouseKeepingTasksByHotelIdUseCase>();

        services.AddScoped<IRequestHandler<GetAllHouseKeepingTasksByRoomIdQuery, List<HouseKeepingTaskDto>>,
                                    GetAllHouseKeepingTasksByRoomIdUseCase>();

        services.AddScoped<IRequestHandler<GetHouseKeepingTaskByIdQuery, HouseKeepingTaskDto>,
                                    GetHouseKeepingTaskByIdUseCase>();


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
        
        services.AddScoped<IRequestHandler<DeleteRoomCommand, bool>, 
                                    DeleteRoomUseCase>();

        services.AddScoped<IRequestHandler<GetAllRoomsQuery, List<RoomDto>>,
                                    GetAllRoomsUseCase>();

        services.AddScoped<IRequestHandler<GetAllRoomsByHotelIdQuery, List<RoomDto>>,
                                    GetAllRoomsByHotelIdUseCase>();

        services.AddScoped<IRequestHandler<GetRoomByIdQuery, RoomDto>,
                                     GetRoomByIdUseCase>();

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
        
        services.AddScoped<IRequestHandler<DeleteRoomTypeCommand, bool>,
            DeleteRoomTypeUseCase>();

        services.AddScoped<IRequestHandler<GetAllRoomTypesQuery, List<RoomTypeDto>>,
            GetAllRoomTypesUseCase>();

        services.AddScoped<IRequestHandler<GetRoomTypeByIdQuery, RoomTypeDto>,
            GetRoomTypeByIdUseCase>();

        // Stays services
        services.AddScoped<IRequestHandler<CompleteStayCommand, bool>,
            CompleteStayUseCase>();

        services.AddScoped<IRequestHandler<CreateStayCommand, Guid>,
            CreateStayUseCase>();

        services.AddScoped<IRequestHandler<IncreaseNightCountCommand, bool>,
            IncreaseNightCountUseCase>();
        
        services.AddScoped<IRequestHandler<DeleteStayCommand, bool>, 
            DeleteStayUseCase>();

        services.AddScoped<IRequestHandler<GetAllStaysQuery, List<StayDto>>,
            GetAllStaysUseCase>();

        services.AddScoped<IRequestHandler<GetAllStaysByHotelIdQuery, List<StayDto>>,
            GetAllStaysByHotelIdUseCase>();

        services.AddScoped<IRequestHandler<GetAllStaysByGuestIdQuery, List<StayDto>>,
            GetAllStaysByGuestIdUseCase>();

        services.AddScoped<IRequestHandler<GetStayByIdQuery, StayDto>,
            GetStayByIdUseCase>();

        // Users services
        services.AddScoped<IRequestHandler<LoginCommand, string>,
                                    LoginUseCase>();
        
        services.AddScoped<IRequestHandler<CreateUserCommand, string>,
                                    CreateUserUseCase>();

        services.AddScoped<IRequestHandler<ChangeEmailCommand, bool>,
                                    ChangeEmailUseCase>();

        services.AddScoped<IRequestHandler<ChangePasswordCommand, bool>,
                                    ChangePasswordUseCase>();

        services.AddScoped<IRequestHandler<ChangePhoneNumberCommand, bool>,
                                    ChangePhoneNumberUseCase>();

        services.AddScoped<IRequestHandler<ChangePreferredLanguageCommand, bool>,
                                    ChangePreferredLanguageUseCase>();

        services.AddScoped<IRequestHandler<ChangeRoleCommand, bool>,
                                    ChangeRoleUseCase>();

        services.AddScoped<IRequestHandler<UpdateUserNameCommand, bool>,
                                    UpdateUserNameUseCase>();

        services.AddScoped<IRequestHandler<GetUserByIdQuery, UserDto>,
                                    GetUserByIdUseCase>();

        services.AddScoped<IRequestHandler<GetAllUsersQuery, List<UserDto>>,
                                    GetAllUsersUseCase>();

        services.AddScoped<IRequestHandler<GetAllUsersByHotelIdQuery, List<UserDto>>,
                                    GetAllUsersByHotelIdUseCase>();


        return services;
    }
}