using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Hotels;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentValidation;
using ValidationException = BrisaPMS.Application.Exceptions.ValidationException;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.CreateHotel;

public class CreateHotelUseCase
{
    private readonly IHotelsRepository _repository;
    private readonly IUnitOfWork  _unitOfWork;
    private readonly IValidator<CreateHotelCommand> _validator;
    
    public CreateHotelUseCase(IHotelsRepository repository, IUnitOfWork unitOfWork, 
        IValidator<CreateHotelCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    
    public async Task<Guid> Handle(CreateHotelCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);
        
        if (validationResult.IsValid is not true)
            throw new ValidationException(validationResult);
        
        var businessEmail = new Email(command.BusinessEmail);
        var businessPhoneNumber = new PhoneNumber(command.BusinessPhoneNumber);
        var logoUrl = new Url(command.LogoUrl!);
        var address = new Address(command.Address1, command.Address2, command.City, command.Province, command.ZipCode);
        var checkInOutTimes = new CheckInOutTimes(command.CheckInTime, command.CheckOutTime);
        var itbisRate = new ItbisRate(command.ItbisRate);
        var serviceChargeRate = new ServiceChargeRate(command.ServiceChargeRate);
        
        var hotel = new Hotel
        (
            command.LegalName,
            command.CommercialName,
            businessEmail,
            businessPhoneNumber,
            address,
            checkInOutTimes,
            itbisRate,
            serviceChargeRate,
            command.IsActive,
            logoUrl,
            command.DefaultCurrencyCode
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