using BrisaPMS.Application.Contracts.Repositories;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelContactInfo;

public class UpdateHotelContactInfoUseCase
{
    private readonly IHotelsRepository _repository;
    
    public UpdateHotelContactInfoUseCase(IHotelsRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateHotelContactInfoCommand command)
    {
        var hotel = await _repository.GetById(command.Id);
        
        if (hotel is null)
            throw new ArgumentException($"Hotel with id {command.Id} not found");
        
        hotel.UpdateBusinessEmail(command.BusinessEmail);
        hotel.UpdateBusinessPhoneNumber(command.BusinessPhoneNumber);
        
        await _repository.Update(hotel);
    }
}
