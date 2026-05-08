using BrisaPMS.API.DTOs.Bookings;
using BrisaPMS.Application.UseCases.Bookings.Commands.CancelBooking;
using BrisaPMS.Application.UseCases.Bookings.Commands.ChangeAssignedRoom;
using BrisaPMS.Application.UseCases.Bookings.Commands.ChangeBookingSource;
using BrisaPMS.Application.UseCases.Bookings.Commands.ConfirmBooking;
using BrisaPMS.Application.UseCases.Bookings.Commands.CreateBooking;
using BrisaPMS.Application.UseCases.Bookings.Commands.DeleteBooking;
using BrisaPMS.Application.UseCases.Bookings.Commands.MarkAsNoShow;
using BrisaPMS.Application.UseCases.Bookings.Commands.UpdateCancellationReason;
using BrisaPMS.Application.UseCases.Bookings.Commands.UpdateCheckInOutTimes;
using BrisaPMS.Application.UseCases.Bookings.Commands.UpdateGuestCount;
using BrisaPMS.Application.UseCases.Bookings.Commands.UpdateSpecialRequests;
using BrisaPMS.Application.UseCases.Bookings.Commands.UpdateTotalPrice;
using BrisaPMS.Application.UseCases.Bookings.Queries.GetAllBookings;
using BrisaPMS.Application.UseCases.Bookings.Queries.GetBookingById;
using BrisaPMS.Application.UseCases.Bookings.Queries.GetBookingsByHotelId;
using BrisaPMS.Application.UseCases.Bookings.Queries.Shared;
using BrisaPMS.Application.Utilities.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrisaPMS.API.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = "AdminManagerReceptionistOnly")]
        public async Task<ActionResult<List<BookingDto>>> GetAll()
        {
            var query = new GetAllBookingsQuery { };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = "AdminManagerReceptionistOnly")]
        public async Task<ActionResult<List<BookingDto>>> GetAllByHotelId([FromBody] GetBookingsByHotelIdQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id:guid}", Name = "GetBookingById")]
        [Authorize(Policy = "AdminManagerReceptionistOnly")]
        public async Task<ActionResult<BookingDto>> GetById([FromRoute] Guid id)
        {
            var query = new GetBookingByIdQuery { BookingId = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "AdminManagerReceptionistOnly")]
        public async Task<IActionResult> Create([FromBody] CreateBookingCommand command)
        {
            var bookingDto = await _mediator.Send(command);
            return CreatedAtRoute("GetBookingById", new { id = bookingDto.Id }, bookingDto);
        }

        [HttpPut("{id:guid}/assigned-room")]
        [Authorize(Policy = "AdminManagerReceptionistOnly")]
        public async Task<IActionResult> ChangeAssignedRoom([FromRoute] Guid id, [FromBody] ChangeAssignedRoomDTO changeAssignedRoomDTO)
        {
            var command = new ChangeAssignedRoomCommand 
            {
                BookingId = id,
                RoomId = changeAssignedRoomDTO.RoomId
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/source")]
        [Authorize(Policy = "AdminManagerReceptionistOnly")]
        public async Task<IActionResult> ChangeBookingSource([FromRoute] Guid id,[FromBody] ChangeBookingSourceDTO changeBookingSourceDTO)
        {
            var command = new ChangeBookingSourceCommand 
            { 
                BookingId = id,
                Source = changeBookingSourceDTO.BookingSource 
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/guest-count")]
        [Authorize(Policy = "AdminManagerReceptionistOnly")]
        public async Task<IActionResult> UpdateGuestCount([FromRoute] Guid id,[FromBody] UpdateGuestCountDTO updateGuestCountDTO)
        {
            var command = new UpdateGuestCountCommand
            {
                BookingId = id,
                NumberOfAdults = updateGuestCountDTO.NumberOfAdults,
                NumberOfChildren = updateGuestCountDTO.NumberOfChildren
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/check-in-out-times")]
        [Authorize(Policy = "AdminManagerReceptionistOnly")]
        public async Task<IActionResult> UpdateCheckInOutTimes([FromRoute] Guid id, [FromBody] UpdateCheckInOutTimesDTO updateCheckInOutTimesDTO)
        {
            var command = new UpdateCheckInOutTimesCommand
            {
                BookingId = id,
                CheckInTime = updateCheckInOutTimesDTO.CheckInTime,
                CheckOutTime = updateCheckInOutTimesDTO.CheckOutTime
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/special-requests")]
        [Authorize(Policy = "AdminManagerReceptionistOnly")]
        public async Task<IActionResult> UpdateSpecialRequests([FromRoute] Guid id, [FromBody] UpdateSpecialRequestDTO updateSpecialRequestDTO)
        {
            var command = new UpdateSpecialRequestsCommand
            {
                BookingId = id,
                SpecialRequests = updateSpecialRequestDTO.SpecialRequest
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/confirm")]
        [Authorize(Policy = "AdminManagerReceptionistOnly")]
        public async Task<IActionResult> Confirm([FromRoute] Guid id)
        {
            var command = new ConfirmBookingCommand { BookingId = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/cancel")]
        [Authorize(Policy = "AdminManagerReceptionistOnly")]
        public async Task<IActionResult> Cancel([FromRoute] Guid id, [FromBody] CancelBookingDTO cancelBookingDTO)
        {
            var command = new CancelBookingCommand
            {
                BookingId = id,
                CancellationReason = cancelBookingDTO.CancellationReason,
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/cancellation-reason")]
        [Authorize(Policy = "AdminManagerReceptionistOnly")]
        public async Task<IActionResult> UpdateCancellationReason([FromRoute] Guid id, [FromBody] CancelBookingDTO cancelBookingDTO)
        {
            var command = new UpdateCancellationReasonCommand
            {
                BookingId = id,
                CancellationReason = cancelBookingDTO.CancellationReason,
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/no-show")]
        [Authorize(Policy = "AdminManagerReceptionistOnly")]
        public async Task<IActionResult> MarkAsNoShow([FromRoute] Guid id)
        {
            var command = new MarkAsNoShowCommand { BookingId = id };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:guid}/total-price")]
        [Authorize(Policy = "AdminManagerReceptionistOnly")]
        public async Task<IActionResult> UpdateTotalPrice([FromRoute] Guid id, [FromBody] UpdateTotalPriceDTO updateTotalPriceDTO)
        {
            var command = new UpdateTotalPriceCommand
            {
                BookingId = id,
                TotalPrice = updateTotalPriceDTO.TotalPrice
            };

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "AdminManagerOnly")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var command = new DeleteBookingCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}