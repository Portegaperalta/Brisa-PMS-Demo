using BrisaPMS.API.DTOs.Housekeeping;
using BrisaPMS.Application.Contracts.Services;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.CancelHouseKeepingTask;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.ChangeHouseKeepingTaskType;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.ChangeTaskDeadline;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.CompleteHouseKeepingTask;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.CreateHouseKeepingTask;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.DeleteHouseKeepingTask;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.ReassignHouseKeepingTask;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.ReportIncident;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.StartHouseKeepingTask;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.UpdateHouseKeepingTaskNotes;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.UpdateHouseKeepingTaskPriority;
using BrisaPMS.Application.UseCases.HouseKeeping.Commands.UpdateIncidentDescription;
using BrisaPMS.Application.UseCases.HouseKeeping.Queries.GetAllHouseKeepingTasks;
using BrisaPMS.Application.UseCases.HouseKeeping.Queries.GetAllHouseKeepingTasksByHotelId;
using BrisaPMS.Application.UseCases.HouseKeeping.Queries.GetAllHouseKeepingTasksByRoomId;
using BrisaPMS.Application.UseCases.HouseKeeping.Queries.GetHouseKeepingTaskById;
using BrisaPMS.Application.UseCases.HouseKeeping.Shared;
using BrisaPMS.Application.Utilities.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrisaPMS.API.Controllers;

[ApiController]
[Route("api/housekeepingTasks")]
[Authorize(Policy = "AdminManagerHouseKeeperOnly")]
public class HouseKeepingTasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public HouseKeepingTasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<HouseKeepingTaskDto>>> GetAll()
    {
        var query = new GetAllHouseKeepingTasksQuery { };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet(Name = "GetAllHouseKeepingTasksByHotelId")]
    public async Task<ActionResult<List<HouseKeepingTaskDto>>> GetAllByHotelId(
        [FromBody] GetAllHouseKeepingTasksByHotelIdQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet(Name = "GetAllHouseKeepingTasksByRoomId")]
    public async Task<ActionResult<List<HouseKeepingTaskDto>>> GetAllByRoomId(
        [FromBody] GetAllHouseKeepingTasksByRoomIdQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = "GetHouseKeepingTaskById")]
    public async Task<ActionResult<HouseKeepingTaskDto>> GetById([FromRoute] Guid id)
    {
        var query = new GetHouseKeepingTaskByIdQuery { HouseKeepingTaskId = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "AdminManagerOnly")]
    public async Task<ActionResult> Create(CreateHouseKeepingTaskCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }

    [HttpPut("{id:guid}/cancel")]
    [Authorize(Policy = "AdminManagerOnly")]
    public async Task<ActionResult> Cancel([FromRoute] Guid id)
    {
        var command = new CancelHouseKeepingTaskCommand { HouseKeepingTaskId = id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/type")]
    [Authorize(Policy = "AdminManagerOnly")]
    public async Task<ActionResult> ChangeType([FromRoute] Guid id, ChangeHouseKeepingTaskTypeDto dto)
    {
        var command = new ChangeHouseKeepingTaskTypeCommand
        {
            HouseKeepingTaskId = id,
            HouseKeepingTaskType = dto.HouseKeepingTaskType
        };
        
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/deadline")]
    [Authorize(Policy = "AdminManagerOnly")]
    public async Task<ActionResult> ChangeDeadline([FromRoute] Guid id, ChangeTaskDeadlineDto dto)
    {
        var command = new ChangeTaskDeadlineCommand
        {
            HouseKeepingTaskId =  id,
            ExpectedStartTime = dto.ExpectedStartTime,
            ExpectedEndTime = dto.ExpectedEndTime
        };
        
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/start")]
    public async Task<IActionResult> Start([FromRoute] Guid id)
    {
        var command = new StartHouseKeepingTaskCommand {HouseKeepingTaskId = id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/complete")]
    public async Task<IActionResult> Complete([FromRoute] Guid id)
    {
        var command = new CompleteHouseKeepingTaskCommand {HouseKeepingTaskId =  id };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/reassign")]
    [Authorize(Policy = "AdminManagerOnly")]
    public async Task<IActionResult> Reassign([FromRoute] Guid id, [FromBody] ReassignTaskDto dto)
    {
        var command = new ReassignHouseKeepingTaskCommand
        {
            AssignedTo = dto.AssignedTo,
            HouseKeepingTaskId = id
        };
        
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/priority")]
    [Authorize(Policy = "AdminManagerOnly")]
    public async Task<IActionResult> UpdatePriority([FromRoute] Guid id, [FromBody] UpdateTaskPriorityDto dto)
    {
        var command = new UpdateHouseKeepingTaskPriorityCommand
        {
            HouseKeepingTaskId = id,
            TaskPriority = dto.TaskPriority
        };
        
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/incident")]
    public async Task<IActionResult> ReportIncident([FromRoute] Guid id, [FromBody] ReportIncidentDto dto)
    {
        var command = new ReportIncidentCommand
        {
            HouseKeepingTaskId = id,
            IncidentDescription = dto.IncidentDescription
        };
        
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/incident-description")]
    public async Task<IActionResult> UpdateIncidentDescription([FromRoute] Guid id, 
        [FromBody] UpdateIncidentDescriptionDto dto)
    {
        var command = new UpdateIncidentDescriptionCommand
        {
            HouseKeepingTaskId = id,
            IncidentDescription = dto.IncidentDescription
        };
        
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{id:guid}/notes")]
    public async Task<IActionResult> UpdateNotes([FromRoute] Guid id, [FromBody] UpdateTaskNotesDto dto)
    {
        var command = new UpdateHouseKeepingTaskNotesCommand
        {
            HouseKeepingTaskId = id,
            Notes = dto.Notes
        };
        
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "AdminManagerOnly")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteHouseKeepingTaskCommand
        {
            Id = id
        };
        
        await _mediator.Send(command);
        return NoContent();
    }
}