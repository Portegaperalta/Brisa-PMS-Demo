using BrisaPMS.API.DTOs.Rooms;
using BrisaPMS.API.Services;
using BrisaPMS.Application.UseCases.Rooms.Commands.ChangeRoomType;
using BrisaPMS.Application.UseCases.Rooms.Commands.CreateRoom;
using BrisaPMS.Application.UseCases.Rooms.Commands.SetAsPendingRestock;
using BrisaPMS.Application.UseCases.Rooms.Commands.SetAsRestocked;
using BrisaPMS.Application.UseCases.Rooms.Commands.UpdateAvailabilityStatus;
using BrisaPMS.Application.UseCases.Rooms.Commands.UpdateHygieneStatus;
using BrisaPMS.Application.UseCases.Rooms.Commands.UpdateRoomNumber;
using BrisaPMS.Application.UseCases.Rooms.Queries.GetAllRooms;
using BrisaPMS.Application.UseCases.Rooms.Queries.GetAllRoomsByHotelId;
using BrisaPMS.Application.UseCases.Rooms.Queries.GetRoomById;
using BrisaPMS.Application.UseCases.Rooms.Shared;
using BrisaPMS.Application.Utilities.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrisaPMS.API.Controllers;

[ApiController]
[Route("api/rooms")]
[Authorize]
public class RoomsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CurrentUserService _currentUserService;

    public RoomsController(IMediator mediator, CurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<List<RoomDto>>> GetAll()
    {
        var query = new GetAllRoomsQuery { };
        var result = await _mediator.Send(query);
        return result;
    }

    [HttpGet(Name = "GetAllRoomsByHotelId")]
    public async Task<ActionResult<List<RoomDto>>> GetAllByHotelId([FromBody] GetAllRoomsByHotelIdQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = "GetRoomById")]
    public async Task<ActionResult<RoomDto>> GetById([FromRoute] Guid id)
    {
        var query = new GetRoomByIdQuery { RoomId = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminManagerOnly")]
    public async Task<IActionResult> Create([FromBody] CreateRoomCommand command)
    {
        var roomDto = await _mediator.Send(command);
        return CreatedAtRoute("GetRoomById", new { id = roomDto.Id }, roomDto);
    }

    [HttpPut("{id:guid}/type")]
    [Authorize(Policy = "AdminManagerOnly")]
    public async Task<IActionResult> UpdateRoomType([FromRoute] Guid id, UpdateRoomTypeDTO updateRoomTypeDTO)
    {
        var command = new ChangeRoomTypeCommand
        {
            RoomId = id,
            RoomTypeId = updateRoomTypeDTO.RoomTypeId,
        };

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/number")]
    [Authorize(Policy = "AdminManagerOnly")]
    public async Task<IActionResult> UpdateNumber([FromRoute] Guid id,
        [FromBody] UpdateRoomNumberDTO updateRoomNumberDTO)
    {
        var command = new UpdateRoomNumberCommand
        {
            RoomId = id,
            Number = updateRoomNumberDTO.RoomNumber
        };

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/availability")]
    [Authorize(Policy = "AdminManagerReceptionistOnly")]
    public async Task<IActionResult> UpdateAvailability([FromRoute] Guid id,
        [FromBody] UpdateAvailabilityDTO updateAvailabilityDto)
    {
        var command = new UpdateAvailabilityStatusCommand
        {
            RoomId = id,
            AvailabilityStatus = updateAvailabilityDto.AvailabilityStatus
        };

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/hygiene-status")]
    public async Task<IActionResult> UpdateHygieneStatus([FromRoute] Guid id,
        [FromBody] UpdateHygieneStatusDTO updateHygieneStatusDTO)
    {
        var currentUserId = _currentUserService.UserId;
        var command = new UpdateHygieneStatusCommand
        {
            RoomId = id,
            HygieneStatus = updateHygieneStatusDTO.HygieneStatus,
            UserId = currentUserId
        };

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/stock/pending")]
    public async Task<IActionResult> SetAsPendingRestock([FromRoute] Guid id)
    {
        var command = new SetAsPendingRestockCommand { RoomId = id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/stock/restocked")]
    public async Task<IActionResult> SetAsRestocked([FromRoute] Guid id)
    {
        var command = new SetAsRestockedCommand { RoomId = id };
        await _mediator.Send(command);
        return NoContent();
    }
}