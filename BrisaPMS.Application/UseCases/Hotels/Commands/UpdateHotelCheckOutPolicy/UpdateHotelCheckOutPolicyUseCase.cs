using BrisaPMS.Application.Contracts.Repositories;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelCheckOutPolicy;

public class UpdateHotelCheckOutPolicyUseCase
{
    private readonly IHotelsRepository _repository;
    
    public UpdateHotelCheckOutPolicyUseCase(IHotelsRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateHotelCheckOutPolicyCommand command)
    {
        var hotel = await _repository.GetById(command.Id);
        
        if (hotel is null)
            throw new ArgumentException($"Hotel with id {command.Id} not found");
        
        hotel.UpdateCheckInOutTimes(command.CheckInOutTimes);
        await _repository.Update(hotel);
    }
}