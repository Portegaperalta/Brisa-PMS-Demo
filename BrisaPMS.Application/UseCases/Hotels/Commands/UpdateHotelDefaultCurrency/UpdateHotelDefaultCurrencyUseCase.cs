using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentValidation;
using ValidationException = BrisaPMS.Application.Exceptions.ValidationException;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelDefaultCurrency;

public class UpdateHotelDefaultCurrencyUseCase : IRequestHandler<UpdateHotelDefaultCurrencyCommand, bool>
{
    private readonly IHotelsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateHotelDefaultCurrencyCommand> _validator;

    public UpdateHotelDefaultCurrencyUseCase(IHotelsRepository repository,  IUnitOfWork unitOfWork,
        IValidator<UpdateHotelDefaultCurrencyCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<bool> Handle(UpdateHotelDefaultCurrencyCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);
        
        if (validationResult.IsValid is not true)
            throw new ValidationException(validationResult);
        
        var hotel = await _repository.GetById(command.HotelId);
        
        if (hotel is null)
            throw new NotFoundException("Hotel", command.HotelId);

        var newDefaultCurrencyCode = (CurrencyCode)Enum.Parse(typeof(CurrencyCode), command.DefaultCurrencyCode);
        
        hotel.UpdateDefaultCurrencyCode(newDefaultCurrencyCode);
        
        try
        {
            await _repository.Update(hotel);
            await _unitOfWork.Persist();
            return true;
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}