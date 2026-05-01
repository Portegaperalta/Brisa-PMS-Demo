using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Contracts.Services;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Users.Commands.ChangePassword;

public class ChangePasswordUseCase : IRequestHandler<ChangePasswordCommand, bool>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IIdentityService _identityService;

    public ChangePasswordUseCase(IUsersRepository usersRepository, IIdentityService identityService)
    {
        _usersRepository = usersRepository;
        _identityService = identityService;
    }

    public async Task<bool> Handle(ChangePasswordCommand command)
    {
        var userExists = await _usersRepository.Exists(command.UserId);
        
        if (userExists is false)
            throw new NotFoundException("User", command.UserId);

        var isCurrentPasswordValid = await _identityService.CheckPasswordAsync(command.UserId, command.CurrentPassword);

        if (isCurrentPasswordValid is not true)
            throw new IncorrectPasswordException();

        await _identityService.UpdatePasswordAsync(command.UserId, command.NewPassword);

        return true;
    }
}