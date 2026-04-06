using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelCheckOutPolicy;

public class UpdateHotelCheckOutPolicyCommand
{
    public required Guid Id { get; set; }
    public DateTime CheckInTime { get; set; }
    public DateTime CheckOutTime { get; set; }
}