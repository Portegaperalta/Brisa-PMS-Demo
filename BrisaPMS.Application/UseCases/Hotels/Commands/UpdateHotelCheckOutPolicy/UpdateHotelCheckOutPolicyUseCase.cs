using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.Hotels;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelCheckOutPolicy;

public class UpdateHotelCheckOutPolicyUseCase : IRequestHandler<UpdateHotelCheckOutPolicyCommand, bool>
{
    private readonly IHotelsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateHotelCheckOutPolicyUseCase(IHotelsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateHotelCheckOutPolicyCommand command)
    {
        var hotel = await _repository.GetById(command.HotelId);
        
        if (hotel is null)
            throw new NotFoundException("Hotel", command.HotelId);

        var newCheckOutPolicy = new CheckOutPolicy(command.CheckInTime, command.CheckOutTime);
        
        hotel.UpdateCheckOutPolicy(newCheckOutPolicy);
        
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