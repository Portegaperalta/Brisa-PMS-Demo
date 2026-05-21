using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Guests.Shared;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Guest;
using BrisaPMS.Domain.Guests;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Guests.Commands.CreateGuest;

public class CreateGuestUseCase : IRequestHandler<CreateGuestCommand, GuestDto>
{
    private readonly IGuestsRepository _guestsRepository;
    private readonly IHotelsRepository _hotelsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGuestUseCase(IGuestsRepository guestsRepository, IHotelsRepository hotelsRepository,
        IUnitOfWork unitOfWork)
    {
        _guestsRepository = guestsRepository;
        _hotelsRepository = hotelsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GuestDto> Handle(CreateGuestCommand command)
    {
        var hotelExists = await _hotelsRepository.Exists(command.HotelId);

        if (hotelExists is not true)
            throw new NotFoundException("Hotel", command.HotelId);
        
        var guestDocumentType = Enum.Parse<GuestDocumentType>(command.DocumentType);
        var email = new Email(command.Email);
        var phoneNumber = new PhoneNumber(command.PhoneNumber);
        var preferredCurrency = Enum.Parse<CurrencyCode>(command.PreferredCurrency);

        var guestBuilder = new Guest.Builder
        (
            command.HotelId,
            command.FirstName,
            command.LastName,
            guestDocumentType,
            command.DocumentNumber,
            email,
            phoneNumber,
            preferredCurrency,
            command.IsVip
        );

        if (string.IsNullOrWhiteSpace(command.Country) is not true)
            guestBuilder.WithCountry(command.Country);

        if (string.IsNullOrWhiteSpace(command.Rnc) is not true)
        {
            var rnc = new Rnc(command.Rnc);
            guestBuilder.WithRnc(rnc);
        }

        if (string.IsNullOrWhiteSpace(command.PreferredLanguage) is not true)
            guestBuilder.WithPreferredLanguage(command.PreferredLanguage);
        
        if (string.IsNullOrWhiteSpace(command.Notes) is not true)
            guestBuilder.WithNotes(command.Notes);

        var guest = guestBuilder.Build();

        try
        {
            await _guestsRepository.Create(guest);
            await _unitOfWork.Persist();
            return guest.ToDto();
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}