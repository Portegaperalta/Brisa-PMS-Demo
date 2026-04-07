using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using FluentValidation;
using ValidationException = BrisaPMS.Application.Exceptions.ValidationException;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.DeactivateHotel;

public class DeactivateHotelUseCase
{
    private readonly IHotelsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeactivateHotelCommand> _validator;

    public DeactivateHotelUseCase(IHotelsRepository repository,  IUnitOfWork unitOfWork,
        IValidator<DeactivateHotelCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task Handle(DeactivateHotelCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);
        
        if (validationResult.IsValid is not true)
            throw new ValidationException(validationResult);
        
        var hotel = await _repository.GetById(command.HotelId);
        
        if (hotel is null)
            throw new ArgumentException($"Hotel with id {command.HotelId} not found");

        try
        {
            hotel.SetAsInactive();
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