using BrisaPMS.API.DTOs.Hotels;
using BrisaPMS.Application.UseCases.Hotels.Commands.ActivateHotel;
using BrisaPMS.Application.UseCases.Hotels.Commands.CreateHotel;
using BrisaPMS.Application.UseCases.Hotels.Commands.DeactivateHotel;
using BrisaPMS.Application.UseCases.Hotels.Commands.DeleteHotel;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelAddressInfo;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelBrandInfo;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelCheckOutPolicy;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelContactInfo;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelDefaultCurrency;
using BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelRates;
using BrisaPMS.Application.UseCases.Hotels.Queries.GetAllHotels;
using BrisaPMS.Application.UseCases.Hotels.Queries.GetHotelById;
using BrisaPMS.Application.UseCases.Hotels.Shared;
using BrisaPMS.Application.Utilities.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrisaPMS.API.Controllers
{
    [ApiController]
    [Route("api/hotels")]
    [Authorize(Policy = "AdminManagerOnly")]
    public class HotelsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HotelsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<HotelDto>>> GetAll([FromBody] GetAllHotelsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id:guid}", Name = "GetHotelById")]
        public async Task<ActionResult<HotelDto>> GetById([FromRoute] Guid id)
        {
            var query = new GetHotelByIdQuery { HotelId = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<HotelDto>> Create([FromBody] CreateHotelCommand command)
        {
            var hotelDto = await _mediator.Send(command);
            return CreatedAtRoute("GetHotelById", new { id = hotelDto.Id }, hotelDto);
        }

        [HttpPut("{id:guid}/brand")]
        public async Task<IActionResult> UpdateBrandInfo([FromRoute] Guid id, [FromBody] UpdateHotelBrandInfoDTO updateHotelBrandInfoDTO)
        {
            var command = new UpdateHotelBrandInfoCommand
            {
                HotelId = id,
                LegalName = updateHotelBrandInfoDTO.LegalName,
                CommercialName = updateHotelBrandInfoDTO.CommercialName,
                LogoUrl = updateHotelBrandInfoDTO.LogoUrl
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/contact")]
        public async Task<IActionResult> UpdateContactInfo([FromRoute] Guid id, [FromBody] UpdateHotelContactInfoDTO updateHotelContactInfoDTO)
        {
            var command = new UpdateHotelContactInfoCommand
            {
                HotelId = id,
                BusinessEmail = updateHotelContactInfoDTO.BusinessEmail,
                BusinessPhoneNumber = updateHotelContactInfoDTO.BusinessPhoneNumber
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/address")]
        public async Task<IActionResult> UpdateAddressInfo([FromRoute] Guid id, [FromBody] UpdateAddressInfoDTO updateAddressInfoDTO)
        {
            var command = new UpdateHotelAddressInfoCommand
            {
                HotelId = id,
                Address1 = updateAddressInfoDTO.Address1,
                Address2 = updateAddressInfoDTO.Address2,
                City = updateAddressInfoDTO.City,
                Province = updateAddressInfoDTO.Province,
                ZipCode = updateAddressInfoDTO.ZipCode
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/check-out-policy")]
        public async Task<IActionResult> UpdateCheckOutPolicy([FromRoute] Guid id, [FromBody] UpdateCheckOutPolicyDTO updateCheckOutPolicyDTO)
        {
            var command = new UpdateHotelCheckOutPolicyCommand
            {
                HotelId = id,
                CheckInTime = updateCheckOutPolicyDTO.CheckInTime,
                CheckOutTime = updateCheckOutPolicyDTO.CheckOutTime,
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/default-currency")]
        public async Task<IActionResult> UpdateDefaultCurrency([FromRoute] Guid id, [FromBody] UpdateDefaultCurrencyDTO updateDefaultCurrencyDTO)
        {
            var command = new UpdateHotelDefaultCurrencyCommand
            {
                HotelId = id,
                DefaultCurrencyCode = updateDefaultCurrencyDTO.CurrencyCode
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/rates")]
        public async Task<IActionResult> UpdateRates([FromRoute] Guid id, [FromBody] UpdateRatesDTO updateRatesDTO)
        {
            var command = new UpdateHotelRatesCommand
            {
                HotelId = id,
                ItbisRate = updateRatesDTO.ItbisRate,
                ServiceChargeRate = updateRatesDTO.ServiceChargeRate
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/deactivate")]
        public async Task<IActionResult> Deactivate([FromRoute] Guid id)
        {
            var command = new DeactivateHotelCommand { HotelId = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/activate")]
        public async Task<IActionResult> Activate([FromRoute] Guid id)
        {
            var command = new ActivateHotelCommand { HotelId = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var command = new DeleteHotelCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}