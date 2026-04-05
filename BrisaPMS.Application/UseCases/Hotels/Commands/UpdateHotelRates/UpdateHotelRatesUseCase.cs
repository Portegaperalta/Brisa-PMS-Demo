using BrisaPMS.Application.Contracts.Repositories;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelRates;

public class UpdateHotelRatesUseCase
{
    private readonly IHotelsRepository _repository;

    public UpdateHotelRatesUseCase(IHotelsRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateHotelRatesCommand command)
    {
        var hotel = await _repository.GetById(command.Id);
        
        if (hotel is null)
            throw new ArgumentException($"Hotel with id {command.Id} not found");
        
        hotel.UpdateItbisRate(command.ItbisRate);
        hotel.UpdateServiceChargeRate(command.ServiceChargeRate);
        await _repository.Update(hotel);
    }
}