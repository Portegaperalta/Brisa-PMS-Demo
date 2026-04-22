using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Users;

namespace BrisaPMS.Application.UseCases.Users.Commands.ChangePreferredLanguage;

public class ChangePreferredLanguageUseCase : IRequestHandler<ChangePreferredLanguageCommand, bool>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePreferredLanguageUseCase(IUsersRepository usersRepository, IUnitOfWork unitOfWork)
    {
        _usersRepository = usersRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ChangePreferredLanguageCommand command)
    {
        var user = await _usersRepository.GetById(command.UserId);
        
        if (user is null)
            throw new NotFoundException("User", command.UserId);

        var newPreferredLanguage = Enum.Parse<UserPreferredLanguage>(command.PreferredLanguage);
        user.UpdatePreferredLanguage(newPreferredLanguage);
        
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