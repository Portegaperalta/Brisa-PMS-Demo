using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotel;

public class UpdateHotelCheckInOutTimesCommand
{
    public required Guid Id { get; set; }
    public required CheckInOutTimes CheckInOutTimes { get; set; }
}