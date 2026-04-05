using BrisaPMS.Application.Contracts.Repositories;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelDefaultCurrency;

public class UpdateHotelDefaultCurrencyUseCase
{
    private readonly IHotelsRepository _repository;

    public UpdateHotelDefaultCurrencyUseCase(IHotelsRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateHotelDefaultCurrencyCommand command)
    {
        var hotel = await _repository.GetById(command.Id);
        
        if (hotel is null)
            throw new ArgumentException($"Hotel with id {command.Id} not found");
        
        hotel.UpdateDefaultCurrencyCode(command.CurrencyCode);
        await _repository.Update(hotel);
    }
}