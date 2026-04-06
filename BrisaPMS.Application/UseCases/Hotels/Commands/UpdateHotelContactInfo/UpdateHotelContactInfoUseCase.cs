using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelContactInfo;

public class UpdateHotelContactInfoUseCase
{
    private readonly IHotelsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateHotelContactInfoUseCase(IHotelsRepository repository,  IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateHotelContactInfoCommand command)
    {
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
