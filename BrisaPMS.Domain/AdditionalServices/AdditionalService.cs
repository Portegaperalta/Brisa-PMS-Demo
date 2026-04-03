using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Domain.AdditionalServices;

public class AdditionalService
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Money Price { get; private set; }
    public CurrencyCode  CurrencyCode { get; private set; }

    public AdditionalService
    (
        string name,
        string description,
        Money price,
        CurrencyCode currencyCode
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new EmptyRequiredFieldException("Additional service name");
        
        if (string.IsNullOrWhiteSpace(description))
            throw new EmptyRequiredFieldException("Additional service description");
        
        if (Enum.IsDefined<CurrencyCode>(currencyCode) is false)
            throw new BusinessRuleException("Invalid currency code");

        Id = Guid.CreateVersion7();
        Name = name;
        Description = description;
        Price = price;
        CurrencyCode = currencyCode;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new EmptyRequiredFieldException("Additional service name");
        
        Name = newName;
    }

    public void UpdateDescription(string newDescription)
    {
        if (string.IsNullOrWhiteSpace(newDescription))
            throw new EmptyRequiredFieldException("Additional service description");
        
        Description = newDescription;
    }

    public void UpdatePrice(Money newPrice) => Price = newPrice;

    public void UpdateCurrencyCode(CurrencyCode newCurrencyCode)
    {
        if (Enum.IsDefined<CurrencyCode>(newCurrencyCode) is false)
            throw new BusinessRuleException("Invalid currency code");
        
        CurrencyCode = newCurrencyCode;
    }
}