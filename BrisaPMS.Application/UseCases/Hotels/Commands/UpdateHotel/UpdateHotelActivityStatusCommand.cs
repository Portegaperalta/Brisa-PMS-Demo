namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotel;

public class UpdateHotelActivityStatusCommand
{
    public required Guid Id { get; set; }
    public required bool IsActive { get; set; }
}