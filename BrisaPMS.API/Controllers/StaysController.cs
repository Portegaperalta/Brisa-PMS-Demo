using BrisaPMS.Application.UseCases.Stays.Commands.CompleteStay;
using BrisaPMS.Application.UseCases.Stays.Commands.CreateStay;
using BrisaPMS.Application.UseCases.Stays.Commands.DeleteStay;
using BrisaPMS.Application.UseCases.Stays.Commands.IncreaseNightCount;
using BrisaPMS.Application.UseCases.Stays.Queries.GetAllStays;
using BrisaPMS.Application.UseCases.Stays.Queries.GetAllStaysByGuestId;
using BrisaPMS.Application.UseCases.Stays.Queries.GetAllStaysByHotelId;
using BrisaPMS.Application.UseCases.Stays.Queries.GetStayById;
using BrisaPMS.Application.UseCases.Stays.Shared;
using BrisaPMS.Application.Utilities.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrisaPMS.API.Controllers
{
    [ApiController]
    [Route("api/stays")]
    [Authorize(Policy = "AdminManagerReceptionistOnly")]
    public class StaysController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StaysController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<StayDto>>> GetAll()
        {
            var query = new GetAllStaysQuery { };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet(Name = "GetAllByGuestId")]
        public async Task<ActionResult<List<StayDto>>> GetAllByGuestId([FromBody] GetAllStaysByGuestIdQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet(Name = "GetAllByHotelId")]
        public async Task<ActionResult<List<StayDto>>> GetAllByHotelId([FromBody] GetAllStaysByHotelIdQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<StayDto>> GetById([FromRoute] Guid id)
        {
            var query = new GetStayByIdQuery { StayId = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStayCommand command)
        {
            await _mediator.Send(command);
            return Created();
        }

        [HttpPut("{id:guid}/complete")]
        public async Task<IActionResult> CompleteStay([FromRoute] Guid id)
        {
            var command = new CompleteStayCommand { StayId = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/night-count")]
        public async Task<IActionResult> IncreaseNightCount([FromRoute] Guid id)
        {
            var command = new IncreaseNightCountCommand { StayId = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var command = new DeleteStayCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}