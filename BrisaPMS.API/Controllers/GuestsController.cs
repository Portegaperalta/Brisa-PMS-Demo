using BrisaPMS.API.DTOs.Guests;
using BrisaPMS.Application.UseCases.Guests.Commands.BlacklistGuest;
using BrisaPMS.Application.UseCases.Guests.Commands.CreateGuest;
using BrisaPMS.Application.UseCases.Guests.Commands.MakeGuestVip;
using BrisaPMS.Application.UseCases.Guests.Commands.RevokeGuestVip;
using BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestContactInfo;
using BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestDocumentation;
using BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestGeneralInfo;
using BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestRnc;
using BrisaPMS.Application.UseCases.Guests.Commands.WhitelistGuest;
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

    [HttpPost]
    public async Task<ActionResult<GuestDto>> Create([FromBody] CreateGuestCommand command)
    {
        var guestDto = await _mediator.Send(command);
        return CreatedAtRoute("GetGuestById", new { guestDto.Id }, guestDto);
    }

    [HttpPut("{id:guid}/general-info")]
    public async Task<IActionResult> UpdateGeneralInfo([FromRoute] Guid id, [FromBody] UpdateGuestGeneralInfoDTO updateGuestGeneralInfoDTO)
    {
        var command = new UpdateGuestGeneralInfoCommand
        {
            GuestId = id,
            FirstName = updateGuestGeneralInfoDTO.FirstName,
            LastName = updateGuestGeneralInfoDTO.LastName,
            Country = updateGuestGeneralInfoDTO.Country,
            PreferredLanguage = updateGuestGeneralInfoDTO.PreferredLanguage,
            Notes = updateGuestGeneralInfoDTO.Notes,
        };
        
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/contact-info")]
    public async Task<IActionResult> UpdateContactInfo([FromRoute] Guid id,
        [FromBody] UpdateGuestContactInfoDTO updateGuestContactInfoDto)
    {
        var command = new UpdateGuestContactInfoCommand
        {
            GuestId = id,
            Email = updateGuestContactInfoDto.Email,
            PhoneNumber = updateGuestContactInfoDto.PhoneNumber,
        };
        
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/documentation")]
    public async Task<IActionResult> UpdateDocumentation([FromRoute] Guid id,
        UpdateGuestDocumentationDTO updateGuestDocumentationDTO)
    {
        var command = new UpdateGuestDocumentationCommand
        {
            GuestId = id,
            DocumentType = updateGuestDocumentationDTO.DocumentType,
            DocumentNumber = updateGuestDocumentationDTO.DocumentNumber
        };
        
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/rnc")]
    public async Task<IActionResult> UpdateRnc([FromRoute] Guid id, [FromBody] UpdateGuestRncDTO UpdateGuestRncDTO)
    {
        var command = new UpdateGuestRncCommand
        {
            GuestId = id,
            Rnc = UpdateGuestRncDTO.Rnc
        };
        
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/blacklist")]
    public async Task<IActionResult> BlackList([FromRoute] Guid id, [FromBody] BlacklistGuestDTO BlacklistGuestDTO)
    {
        var command = new BlacklistGuestCommand
        {
            GuestId = id,
            BlacklistedReason = BlacklistGuestDTO.BlacklistedReason
        };
        
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/whitelist")]
    public async Task<IActionResult> WhiteList([FromRoute] Guid id)
    {
        var command = new WhitelistGuestCommand { GuestId = id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/vip/add")]
    public async Task<IActionResult> MakeVIp([FromRoute] Guid id)
    {
        var command = new MakeGuestVipCommand {GuestId = id};
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/vip/revoke")]
    public async Task<IActionResult> RevokeVip([FromRoute] Guid id)
    {
        var command = new RevokeGuestVipCommand {GuestId =  id};
        await _mediator.Send(command);
        return NoContent();
    }
}