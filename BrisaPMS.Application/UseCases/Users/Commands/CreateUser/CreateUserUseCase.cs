using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Shared.ValueObjects;
using BrisaPMS.Domain.Users;

namespace BrisaPMS.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserUseCase : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IHotelsRepository _hotelsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserUseCase(IUsersRepository usersRepository, IHotelsRepository hotelsRepository,
        IUnitOfWork unitOfWork)
    {
        _usersRepository = usersRepository;
        _hotelsRepository = hotelsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateUserCommand command)
    {
        if (command.HotelId is not null)
        {
            var hotelExists = await _hotelsRepository.Exists(command.HotelId.Value);
            
            if (hotelExists is not true)
                throw new NotFoundException("Hotel", command.HotelId.Value);
        }
        
        var role = Enum.Parse<UserRole>(command.Role);
        var email = new Email(command.Email);
        var preferredLanguage = Enum.Parse<UserPreferredLanguage>(command.PreferredLanguage);

        var userBuilder = new User.Builder
        (
            role,
            command.FirstName,
            command.LastName,
            email,
            preferredLanguage
        );
        
        if (command.HotelId is not null)
            userBuilder.WithHotelId(command.HotelId.Value);

        if (command.PhoneNumber is not null)
        {
            var phoneNumber = new PhoneNumber(command.PhoneNumber);
            userBuilder.WithPhoneNumber(phoneNumber);
        }

        var user = userBuilder.Build();

        try
        {
            await _usersRepository.Create(user);
            await _unitOfWork.Persist();
            return user.Id;
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}     