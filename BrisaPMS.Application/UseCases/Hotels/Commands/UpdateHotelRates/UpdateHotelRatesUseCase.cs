using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Billing;
using FluentValidation;
using ValidationException = BrisaPMS.Application.Exceptions.ValidationException;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelRates;

public class UpdateHotelRatesUseCase : IRequestHandler<UpdateHotelRatesCommand, bool>
{
    private readonly IHotelsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateHotelRatesCommand> _validator;

    public UpdateHotelRatesUseCase(IHotelsRepository repository, IUnitOfWork unitOfWork,
        IValidator<UpdateHotelRatesCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<bool> Handle(UpdateHotelRatesCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);
        
        if (validationResult.IsValid is not true)
            throw new ValidationException(validationResult);
        
        var hotel = await _repository.GetById(command.HotelId);
        
        if (hotel is null)
            throw new NotFoundException("Hotel", command.HotelId);

        var newItbisRate = new ItbisRate(command.ItbisRate);
        var newServiceChargeRate = new ServiceChargeRate(command.ServiceChargeRate);
        
        hotel.UpdateItbisRate(newItbisRate);
        hotel.UpdateServiceChargeRate(newServiceChargeRate);

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