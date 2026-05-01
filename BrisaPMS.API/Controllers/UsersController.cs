using BrisaPMS.API.DTOs.Users;
using BrisaPMS.Application.UseCases.Users.Commands.CreateUser;
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
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO createUserDTO)
        {
            var command = new CreateUserCommand
            {
                Role = createUserDTO.Role,
                HotelId = createUserDTO.HotelId,
                FirstName = createUserDTO.FirstName,
                LastName = createUserDTO.LastName,
                Email = createUserDTO.Email,
                Password = createUserDTO.Password,
                PhoneNumber = createUserDTO.PhoneNumber,
                PreferredLanguage = createUserDTO.PreferredLanguage
            };

            await _mediator.Send(command);
            return Ok();
        }
    }
}