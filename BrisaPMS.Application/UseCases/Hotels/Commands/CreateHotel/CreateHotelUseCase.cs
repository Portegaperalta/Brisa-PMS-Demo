using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Hotels;
using BrisaPMS.Domain.Shared.Enums; 
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.CreateHotel;

public class CreateHotelUseCase : IRequestHandler<CreateHotelCommand, Guid>
{
    private readonly IHotelsRepository _repository;
    private readonly IUnitOfWork  _unitOfWork;
    
    public CreateHotelUseCase(IHotelsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Guid> Handle(CreateHotelCommand command)
    {
        var businessEmail = new Email(command.BusinessEmail);
        var businessPhoneNumber = new PhoneNumber(command.BusinessPhoneNumber);
        var logoUrl = new Url(command.LogoUrl!);
        var address = new Address(command.Address1, command.Address2, command.City, command.Province, command.ZipCode);
        var checkOutPolicy = new CheckOutPolicy(command.CheckInTime,  command.CheckOutTime);
        var defaultCurrencyCode = Enum.Parse<CurrencyCode>(command.DefaultCurrencyCode, ignoreCase: true);
        var itbisRate = new ItbisRate(command.ItbisRate);
        var serviceChargeRate = new ServiceChargeRate(command.ServiceChargeRate);
        
        var hotel = new Hotel
        (
            command.LegalName,
            command.CommercialName,
            businessEmail,
            businessPhoneNumber,
            address,
            checkOutPolicy,
            itbisRate,
            serviceChargeRate,
            command.IsActive,
            logoUrl,
            defaultCurrencyCode
        );
        
        try
        {
            var response = await _repository.Create(hotel);
            await _unitOfWork.Persist();
            return response.Id;
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}