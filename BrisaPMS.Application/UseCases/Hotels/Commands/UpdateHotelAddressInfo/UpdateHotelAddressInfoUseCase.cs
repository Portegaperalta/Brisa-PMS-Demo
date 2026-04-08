using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.Shared.ValueObjects;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelAddressInfo;

public class UpdateHotelAddressInfoUseCase : IRequestHandler<UpdateHotelAddressInfoCommand, bool>
{
    private  readonly IHotelsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateHotelAddressInfoUseCase(IHotelsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateHotelAddressInfoCommand command)
    {
        var hotel = await _repository.GetById(command.HotelId);
        
        if (hotel is null)
            throw new NotFoundException("Hotel", command.HotelId);
        
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
            return true;
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}