using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.Hotels;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.CreateHotel;

public class CreateHotelUseCase
{
    private readonly IHotelsRepository _repository;
    private readonly IUnitOfWork  _unitOfWork;
    
    public CreateHotelUseCase(IHotelsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Guid> Handle(CreateHotelCommand command)
    {
        try
        {
            var hotel = new Hotel
            (
                command.LegalName,
                command.CommercialName,
                command.BusinessEmail,
                command.BusinessPhoneNumber,
                command.Address,
                command.CheckInOutTimes,
                command.ItbisRate,
                command.ServiceChargeRate,
                command.IsActive,
                command.LogoUrl,
                command.DefaultCurrencyCode
            );
            
            var response = await _repository.Create(hotel);
            await _unitOfWork.Persist();
            return response.Id;
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}