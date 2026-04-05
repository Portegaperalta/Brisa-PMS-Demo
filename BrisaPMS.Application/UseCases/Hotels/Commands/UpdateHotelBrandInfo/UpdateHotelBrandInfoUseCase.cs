using BrisaPMS.Application.Contracts.Repositories;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelBrandInfo;

public class UpdateHotelBrandInfoUseCase
{
    private readonly IHotelsRepository _repository;

    public UpdateHotelBrandInfoUseCase(IHotelsRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateHotelBrandInfoCommand command)
    {
        var hotel = await _repository.GetById(command.Id);
        
        if (hotel == null)
            throw new ArgumentException($"Hotel with id {command.Id} not found");
        
        hotel.UpdateLegalName(command.LegalName);
        hotel.UpdateCommercialName(command.CommercialName);
        hotel.UpdateLogoUrl(command.LogoUrl!);
        await _repository.Update(hotel);
    }
}