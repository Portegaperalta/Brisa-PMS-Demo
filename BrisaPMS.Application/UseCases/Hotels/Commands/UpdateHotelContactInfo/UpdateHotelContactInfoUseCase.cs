using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelContactInfo;

public class UpdateHotelContactInfoUseCase : IRequestHandler<UpdateHotelContactInfoCommand, bool>
{
    private readonly IHotelsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateHotelContactInfoUseCase(IHotelsRepository repository,  IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateHotelContactInfoCommand command)
    {
        var hotel = await _repository.GetById(command.HotelId);
        
        if (hotel is null)
            throw new NotFoundException("Hotel",command.HotelId);
        
        var newBusinessEmail = new Email(command.BusinessEmail);
        var newBusinessPhoneNumber = new PhoneNumber(command.BusinessPhoneNumber);
        
        hotel.UpdateBusinessEmail(newBusinessEmail);
        hotel.UpdateBusinessPhoneNumber(newBusinessPhoneNumber);
        
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
