using BrisaPMS.API.DTOs.RoomTypes;
using BrisaPMS.Application.UseCases.RoomTypes.Commands.CreateRoomType;
using BrisaPMS.Application.UseCases.RoomTypes.Commands.DeleteRoomType;
using BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeBaseRate;
using BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeBedsInfo;
using BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeGeneralInfo;
using BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeOccupancyPolicy;
using BrisaPMS.Application.UseCases.RoomTypes.Queries.GetAllRoomTypes;
using BrisaPMS.Application.UseCases.RoomTypes.Queries.GetRoomTypeById;
using BrisaPMS.Application.UseCases.RoomTypes.Shared;
using BrisaPMS.Application.Utilities.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrisaPMS.API.Controllers;

[ApiController]
[Route("api/room-types")]
[Authorize()]
public class RoomTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoomTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<RoomTypeDto>>> GetAll()
    {
        var query = new GetAllRoomTypesQuery { };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = "GetRoomTypeById")]
    public async Task<ActionResult<RoomTypeDto>> GetById([FromRoute] Guid id)
    {
        var query = new GetRoomTypeByIdQuery { RoomTypeId = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<RoomTypeDto>> Create([FromBody] CreateRoomTypeCommand command)
    {
        var roomTypeDto = await  _mediator.Send(command);
        return CreatedAtRoute("GetRoomTypeById", new { id = roomTypeDto.Id }, roomTypeDto);
    }

    [HttpPut("{id:guid}/general-info")]
    public async Task<IActionResult> UpdateGeneralInfo([FromRoute] Guid id, 
        [FromBody] UpdateRoomTypeGeneralInfoDTO updateRoomTypeGeneralInfoDto)
    {
        var command = new UpdateRoomTypeGeneralInfoCommand
        {
            RoomTypeId = id,
            Name = updateRoomTypeGeneralInfoDto.Name,
            Description = updateRoomTypeGeneralInfoDto.Description
        };

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/base-rate")]
    public async Task<IActionResult> UpdateBaseRate([FromRoute] Guid id,
        [FromBody] UpdateRoomTypeBaseRateDTO updateRoomTypeBaseRateDto)
    {
        var command = new UpdateRoomTypeBaseRateCommand
        {
            RoomTypeId = id,
            NewBaseRate = updateRoomTypeBaseRateDto.NewBaseRate,
        };
        
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/beds")]
    public async Task<IActionResult> UpdateBedsInfo([FromRoute] Guid id,
        [FromBody] UpdateRoomTypeBedsInfoDTO updateRoomTypeBedsInfoDto)
    {
        var command = new UpdateRoomTypeBedsInfoCommand
        {
            RoomTypeId = id,
            BedType = updateRoomTypeBedsInfoDto.BedType,
            NumberOfBeds = updateRoomTypeBedsInfoDto.NumberOfBeds,
        };
        
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/occupancy-policy")]
    public async Task<IActionResult> UpdateOccupancyPolicy([FromRoute] Guid id,
        [FromBody] UpdateRoomTypeOccupancyPolicyDTO updateRoomTypeOccupancyPolicyDto)
    {
        var command = new UpdateRoomTypeOccupancyPolicyCommand
        {
            RoomTypeId = id,
            MaxOccupancyAdults = updateRoomTypeOccupancyPolicyDto.MaxOccupancyAdults,
            MaxOccupancyChildren = updateRoomTypeOccupancyPolicyDto.MaxOccupancyChildren
        };
        
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteRoomTypeCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}