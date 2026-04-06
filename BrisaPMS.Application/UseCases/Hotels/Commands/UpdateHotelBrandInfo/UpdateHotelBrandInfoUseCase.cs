using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentValidation;
using ValidationException = BrisaPMS.Application.Exceptions.ValidationException;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelBrandInfo;

public class UpdateHotelBrandInfoUseCase
{
    private readonly IHotelsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateHotelBrandInfoCommand> _validator;

    public UpdateHotelBrandInfoUseCase(IHotelsRepository repository,  IUnitOfWork unitOfWork,
        IValidator<UpdateHotelBrandInfoCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task Handle(UpdateHotelBrandInfoCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);
        
        if (validationResult.IsValid is not true)
            throw new ValidationException(validationResult);
        
        var hotel = await _repository.GetById(command.Id);
        
        if (hotel is null)
            throw new HotelNotFoundException(command.Id);

        try
        {
            hotel.UpdateLegalName(command.LegalName);
            hotel.UpdateCommercialName(command.CommercialName);
            
            if (command.LogoUrl is not null)
            {
                var newLogo = new Url(command.LogoUrl);
                hotel.UpdateLogoUrl(newLogo);
            }
            
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