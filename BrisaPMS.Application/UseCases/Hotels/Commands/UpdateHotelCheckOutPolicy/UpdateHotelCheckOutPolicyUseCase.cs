using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.Hotels;
using FluentValidation;
using ValidationException = BrisaPMS.Application.Exceptions.ValidationException;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelCheckOutPolicy;

public class UpdateHotelCheckOutPolicyUseCase
{
    private readonly IHotelsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateHotelCheckOutPolicyCommand> _validator;
    
    public UpdateHotelCheckOutPolicyUseCase(IHotelsRepository repository, IUnitOfWork unitOfWork,
        IValidator<UpdateHotelCheckOutPolicyCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task Handle(UpdateHotelCheckOutPolicyCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);
        
        if (validationResult.IsValid is not true)
            throw new ValidationException(validationResult);
        
        var hotel = await _repository.GetById(command.HotelId);
        
        if (hotel is null)
            throw new ArgumentException($"Hotel with id {command.HotelId} not found");
        
        var newCheckOutPolicy = new CheckOutPolicy(command.CheckInTime, command.CheckOutTime);
        
        try
        {
            hotel.UpdateCheckOutPolicy(newCheckOutPolicy);
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