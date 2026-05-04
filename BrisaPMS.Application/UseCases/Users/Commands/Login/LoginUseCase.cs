using BrisaPMS.Application.Contracts.Services;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Users.Commands.Login
{
    public class LoginUseCase : IRequestHandler<LoginCommand, string>
    {
        private readonly IIdentityService _identityService;
        
        public LoginUseCase(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<string> Handle(LoginCommand command)
        {
            return await _identityService.LoginAsync(command.Email, command.Password);
        }
    }
}