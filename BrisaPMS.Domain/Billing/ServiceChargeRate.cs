namespace BrisaPMS.Domain.Billing;

public record ServiceChargeRate
{
    public decimal Rate { get; }

    private ServiceChargeRate() { }

    public ServiceChargeRate(decimal rate)
    {
        if (rate is < 0 or > 100)
            throw new InvalidServiceChargeRateException("Service charge rate must be between 0% and 100%");
        
        Rate = rate;
    }
};