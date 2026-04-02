using System.Text.RegularExpressions;
using BrisaPMS.Domain.Enums;
using BrisaPMS.Domain.Exceptions;

namespace BrisaPMS.Domain.ValueObjects;

public record Ncf
{
    public string Value { get; }
    
    private static readonly Regex Format = new(@"^(B0[1-9]|B[1-9]\d)-\d{11}$",  RegexOptions.Compiled);

    public Ncf(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new BusinessRuleException("NCF can't be empty");
        
        if (Format.IsMatch(value) is not true)
            throw new BusinessRuleException($"Invalid NCF format: {value}");
        
        Value = value.ToUpper();
    }

    public string Series => Value[..3];
    
    public string Sequence => Value[..4];

    public InvoiceType Type => Series switch
    {
        "B01" => InvoiceType.FacturaCredito,
        "B02" => InvoiceType.FacturaConsumo,
        "B04" => InvoiceType.NotaDeCredito,
        "B15" => InvoiceType.NotaDeDebito,
        "B14" => InvoiceType.RegimeEspecial,
        _ => throw new BusinessRuleException($"Unknown NCF type: {Series}")
    };
}