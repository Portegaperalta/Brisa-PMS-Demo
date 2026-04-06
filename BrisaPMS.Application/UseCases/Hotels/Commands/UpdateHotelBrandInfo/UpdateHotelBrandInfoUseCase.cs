using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelBrandInfo;

public class UpdateHotelBrandInfoUseCase
{
    private readonly IHotelsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateHotelBrandInfoUseCase(IHotelsRepository repository,  IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateHotelBrandInfoCommand command)
    {
        var hotel = await _repository.GetById(command.Id);
        
        if (hotel == null)
            throw new ArgumentException($"Hotel with id {command.Id} not found");

        try
        {
            hotel.UpdateLegalName(command.LegalName);
            hotel.UpdateCommercialName(command.CommercialName);
            hotel.UpdateLogoUrl(command.LogoUrl!);
            await _repository.Update(hotel);
            await _unitOfWork.Persist();
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}