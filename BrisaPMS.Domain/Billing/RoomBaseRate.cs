using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.Billing;

public record RoomBaseRate
{
    public decimal Rate { get; }
    
    private RoomBaseRate() { }

    public RoomBaseRate(decimal rate)
    {
        if (rate is < 0 or > 100)
            throw new BusinessRuleException("Base Rate must be between 0% and 100%");
        
        Rate = rate;
    }
}