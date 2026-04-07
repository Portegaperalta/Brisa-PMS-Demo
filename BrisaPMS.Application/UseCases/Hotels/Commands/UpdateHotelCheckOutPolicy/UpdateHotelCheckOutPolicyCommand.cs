using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelCheckOutPolicy;

public class UpdateHotelCheckOutPolicyCommand
{
    public required Guid HotelId { get; set; }
    public TimeOnly CheckInTime { get; set; }
    public TimeOnly CheckOutTime { get; set; }
}