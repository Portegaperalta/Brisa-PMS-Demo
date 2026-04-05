using BrisaPMS.Application.Contracts.Repositories;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.DeactivateHotel;

public class DeactivateHotelUseCase
{
    private readonly IHotelsRepository _repository;

    public DeactivateHotelUseCase(IHotelsRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeactivateHotelCommand command)
    {
        var hotel = await _repository.GetById(command.Id);
        
        if (hotel is null)
            throw new ArgumentException($"Hotel with id {command.Id} not found");
        
        hotel.SetAsInactive();
        await _repository.Update(hotel);
    }
}