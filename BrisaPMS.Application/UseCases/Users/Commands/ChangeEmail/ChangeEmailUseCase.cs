using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Contracts.Services;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Users.Commands.ChangeEmail;

public class ChangeEmailUseCase : IRequestHandler<ChangeEmailCommand, bool>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IIdentityService _identityService;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeEmailUseCase(IUsersRepository usersRepository, IUnitOfWork unitOfWork, 
        IIdentityService identityService)
    {
        _usersRepository = usersRepository;
        _identityService = identityService;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ChangeEmailCommand command)
    {
        var user = await _usersRepository.GetById(command.UserId);
        
        if (user is null)
            throw new NotFoundException("User", command.UserId);

        var isEmailUnique = await _identityService.IsEmailUniqueAsync(command.Email);

        if (isEmailUnique is not true)
            throw new InvalidOperationException("Email already in use");

        var newEmail = new Email(command.Email);
        user.ChangeEmail(newEmail);
        
        try
        {
            await _usersRepository.Update(user);
            await _identityService.UpdateEmailAsync(command.UserId, command.Email);
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