using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentValidation;
using ValidationException = BrisaPMS.Application.Exceptions.ValidationException;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelContactInfo;

public class UpdateHotelContactInfoUseCase
{
    private readonly IHotelsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateHotelContactInfoCommand> _validator;
    
    public UpdateHotelContactInfoUseCase(IHotelsRepository repository,  IUnitOfWork unitOfWork, 
        IValidator<UpdateHotelContactInfoCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task Handle(UpdateHotelContactInfoCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);
        
        if (validationResult.IsValid is not true)
            throw new ValidationException(validationResult);
        
        var hotel = await _repository.GetById(command.Id);
        
        if (hotel is null)
            throw new HotelNotFoundException(command.Id);
        
        var newBusinessEmail = new Email(command.BusinessEmail);
        var newBusinessPhoneNumber = new PhoneNumber(command.BusinessPhoneNumber);

        try
        {
            hotel.UpdateBusinessEmail(newBusinessEmail);
            hotel.UpdateBusinessPhoneNumber(newBusinessPhoneNumber);
            await _repository.Update(hotel);
            await _unitOfWork.Persist();
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}
