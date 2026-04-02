using BrisaPMS.Domain.ValueObjects;

namespace BrisaPMS.Domain.Entities;

public class Discount
{
    public Guid Id { get; }
    public Guid HotelId { get; init; }
    public string Name { get; private set; }
    public string Type { get; private set; }
    public decimal Value { get;  private set; }
    public DiscountTimeInterval TimeInterval { get; private set; }
    public bool IsActive { get; private set; }
}