using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Contracts.Services;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Users;

namespace BrisaPMS.Application.UseCases.Users.Commands.ChangeRole;

public class ChangeRoleUseCase : IRequestHandler<ChangeRoleCommand, bool>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IIdentityService _identityService;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeRoleUseCase(IUsersRepository usersRepository, IIdentityService identityService,
        IUnitOfWork unitOfWork)
    {
        _usersRepository = usersRepository;
        _identityService = identityService;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ChangeRoleCommand command)
    {
        var user = await _usersRepository.GetById(command.UserId);

        if (user is null)
            throw new NotFoundException("User", command.UserId);
        
        var newRole = Enum.Parse<UserRole>(command.Role);
        
        user.ChangeRole(newRole);

        try
        {
            await _usersRepository.Update(user);
            await _identityService.AssignRoleAsync(command.UserId, newRole);
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