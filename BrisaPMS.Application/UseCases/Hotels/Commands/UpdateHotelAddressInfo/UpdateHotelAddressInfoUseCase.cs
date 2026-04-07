using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.Shared.ValueObjects;
using FluentValidation;
using ValidationException = BrisaPMS.Application.Exceptions.ValidationException;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelAddressInfo;

public class UpdateHotelAddressInfoUseCase
{
    private  readonly IHotelsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateHotelAddressInfoCommand> _validator;

    public UpdateHotelAddressInfoUseCase(IHotelsRepository repository, IUnitOfWork unitOfWork,
        IValidator<UpdateHotelAddressInfoCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task Handle(UpdateHotelAddressInfoCommand command)
    {
        var validationResult = await _validator.ValidateAsync(command);
        
        if (validationResult.IsValid is not true)
            throw new ValidationException(validationResult);
        
        var hotel = await _repository.GetById(command.HotelId);
        
        if (hotel is null)
            throw new ArgumentException($"Hotel with id {command.HotelId} not found");
        
        var newAddress = new Address
            (
                command.Address1, 
                command.Address2, 
                command.City, 
                command.Province,
                command.ZipCode
            );

        try
        {
            hotel.UpdateAddress(newAddress);
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