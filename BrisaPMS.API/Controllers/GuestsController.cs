using BrisaPMS.Application.UseCases.Guests.Queries.GetAllGuestsByHotelId;
using BrisaPMS.Application.UseCases.Guests.Queries.GetGuestById;
using BrisaPMS.Application.UseCases.Guests.Shared;
using BrisaPMS.Application.Utilities.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrisaPMS.API.Controllers;

[ApiController]
[Route("api/guests")]
[Authorize(Policy = "AdminManagerReceptionistOnly")]
public class GuestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GuestsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<GuestDto>>> GetAllByHotelId([FromBody] Guid hotelId)
    {
        var query = new GetAllGuestsByHotelIdQuery { HotelId = hotelId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = "GetGuestById")]
    public async Task<ActionResult<GuestDto>> GetById([FromRoute] Guid id)
    {
        var query = new GetGuestByIdQuery { GuestId = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}