using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Users.Commands.ChangeEmail;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Users.Commands.ChangePhoneNumber;

public class ChangePhoneNumberUseCase : IRequestHandler<ChangePhoneNumberCommand, bool>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePhoneNumberUseCase(IUsersRepository usersRepository, IUnitOfWork unitOfWork)
    {
        _usersRepository = usersRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ChangePhoneNumberCommand command)
    {
        var user = await _usersRepository.GetById(command.UserId);
        
        if (user is null)
            throw new NotFoundException("User", command.UserId);

        var newPhoneNumber = new PhoneNumber(command.PhoneNumber);
        user.ChangePhoneNumber(newPhoneNumber);
        
        try
        {
            await _usersRepository.Update(user);
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