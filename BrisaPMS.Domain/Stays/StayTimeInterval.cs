using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.Stays;

public record StayTimeInterval
{
    public DateTime ActualCheckIn { get; }
    public DateTime? ActualCheckOut { get; }

    public StayTimeInterval(DateTime actualCheckIn, DateTime? actualCheckOut = null)
    {
        if (actualCheckIn > actualCheckOut)
            throw new BusinessRuleException("Actual Check-In must be earlier than Actual Check-Out");
        
        if  (actualCheckOut < actualCheckIn)
            throw new BusinessRuleException("Actual Check-Out must be later than Actual Check-In");
        
        ActualCheckIn = actualCheckIn;
        ActualCheckOut = actualCheckOut;
    }
}