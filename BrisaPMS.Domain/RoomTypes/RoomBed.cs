using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Domain.RoomTypes;

public record RoomBed
{
    public BedType BedType { get; }
    public int NumberOfBeds { get; }

    private RoomBed() { }

    public RoomBed(BedType bedType, int numberOfBeds)
    { 
        if (Enum.IsDefined<BedType>(bedType) is not true) 
            throw new BusinessRuleException("Bed Type not supported");
        
        if (numberOfBeds <= 0)
                throw new BusinessRuleException("Number of Beds must be greater than zero");
        
        BedType = bedType;
        NumberOfBeds = numberOfBeds;
    }
}