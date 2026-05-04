using BrisaPMS.API.DTOs.Users;
using BrisaPMS.Application.UseCases.Users.Commands.CreateUser;
using BrisaPMS.Application.UseCases.Users.Commands.Login;
using BrisaPMS.Application.Utilities.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace BrisaPMS.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}