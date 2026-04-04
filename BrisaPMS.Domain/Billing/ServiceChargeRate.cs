namespace BrisaPMS.Domain.Billing;

public record ServiceChargeRate
{
    public decimal Rate { get; }

    public ServiceChargeRate(decimal rate)
    {
        if (rate < 0)
            throw new InvalidServiceChargeRateException("Service charge rate can't be negative");
        
        if (rate > 100)
            throw new InvalidServiceChargeRateException("Service charge rate can't be over 100");

        Rate = rate;
    }
};