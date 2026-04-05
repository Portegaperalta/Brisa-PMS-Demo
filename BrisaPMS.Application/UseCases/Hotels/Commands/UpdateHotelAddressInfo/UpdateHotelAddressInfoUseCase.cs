using BrisaPMS.Application.Contracts.Repositories;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelAddressInfo;

public class UpdateHotelAddressInfoUseCase
{
    private  readonly IHotelsRepository _repository;

    public UpdateHotelAddressInfoUseCase(IHotelsRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateHotelAddressInfoCommand command)
    {
        var hotel = await _repository.GetById(command.Id);
        
        if (hotel is null)
            throw new ArgumentException($"Hotel with id {command.Id} not found");
        
        hotel.UpdateAddress(command.Address);
        await _repository.Update(hotel);
    }
}