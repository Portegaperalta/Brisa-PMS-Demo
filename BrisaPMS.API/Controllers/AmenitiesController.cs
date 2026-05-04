using BrisaPMS.API.DTOs.Amenities;
using BrisaPMS.Application.UseCases.Amenities.Commands.ActivateAmenity;
using BrisaPMS.Application.UseCases.Amenities.Commands.CreateAmenity;
using BrisaPMS.Application.UseCases.Amenities.Commands.DeactivateAmenity;
using BrisaPMS.Application.UseCases.Amenities.Commands.DeleteAmenity;
using BrisaPMS.Application.UseCases.Amenities.Commands.UpdateAmenityDetails;
using BrisaPMS.Application.UseCases.Amenities.Queries.GetAllAmenities;
using BrisaPMS.Application.UseCases.Amenities.Queries.GetAmenityById;
using BrisaPMS.Application.UseCases.Amenities.Shared;
using BrisaPMS.Application.Utilities.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrisaPMS.API.Controllers
{
    [ApiController]
    [Route("api/amenities")]
    public class AmenitiesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AmenitiesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<AmenityDto>>> GetAll()
        {
            var query = new GetAllAmenitiesQuery { };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id:guid}", Name = "GetAmenityById")]
        [Authorize]
        public async Task<ActionResult<AmenityDto>> GetById([FromRoute] Guid id)
        {
            var query = new GetAmenityByIdQuery { AmenityId = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "isAdminOrManager")]
        public async Task<IActionResult> Create([FromBody] CreateAmenityDTO createAmenityDTO)
        {
            var command = new CreateAmenityCommand
            {
                Name = createAmenityDTO.Name,
                Description = createAmenityDTO.Description,
                IsActive = createAmenityDTO.IsActive
            };

            var amenityDto = await _mediator.Send(command);

            return CreatedAtRoute("GetAmenityById", new {id = amenityDto.Id}, amenityDto);
        }

        [HttpPut]
        [Authorize(Policy = "isAdminOrManager")]
        public async Task<IActionResult> UpdateDetails([FromRoute] Guid id, [FromBody] UpdateAmenityDetailsDTO updateAmenityDetailsDTO)
        {
            var command = new UpdateAmenityDetailsCommand
            {
                AmenityId = id,
                Name = updateAmenityDetailsDTO.Name,
                Description = updateAmenityDetailsDTO.Description
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/deactivate")]
        [Authorize(Policy = "isAdminOrManager")]
        public async Task<IActionResult> Deactivate([FromRoute] Guid id)
        {
            var command = new DeactivateAmenityCommand { AmenityId = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/activate")]
        [Authorize(Policy = "isAdminOrManager")]
        public async Task<IActionResult> Activate([FromRoute] Guid id)
        {
            var command = new ActivateAmenityCommand { AmenityId = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "isAdminOrManager")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var command = new DeleteAmenityCommand { Id =  id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}