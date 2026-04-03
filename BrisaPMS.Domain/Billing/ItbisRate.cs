namespace BrisaPMS.Domain.Billing;

public record ItbisRate
{
    public decimal Rate { get; }

    public ItbisRate(decimal rate)
    {
        if (rate < 0)
            throw new InvalidItbisRateException("ITBIS rate can't be negative");
        
        if (rate > 100)
            throw new InvalidItbisRateException("ITBIS rate can't be greater than 100");
        
        Rate = rate;
    }
}