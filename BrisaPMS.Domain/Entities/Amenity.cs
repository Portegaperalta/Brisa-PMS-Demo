using BrisaPMS.Domain.Exceptions;

namespace BrisaPMS.Domain.Entities;

public class Amenity
{
    public Guid Id { get; init; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }

    public Amenity(string name, string description, bool isActive = true)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new EmptyRequiredFieldException("Amenity name");
        
        if (string.IsNullOrWhiteSpace(description))
            throw new EmptyRequiredFieldException("Amenity description");
        
        Name = name;
        Description = description;
        IsActive = isActive;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new EmptyRequiredFieldException("Amenity name");
        
        Name = newName;
    }

    public void UpdateDescription(string newDescription)
    {
        if (string.IsNullOrWhiteSpace(newDescription))
            throw new EmptyRequiredFieldException("Amenity description");
        
        Description = newDescription;
    }

    public void SetAsActive()
    {
        if (IsActive is false)
            IsActive = true;
    }

    public void SetAsInactive()
    {
        if (IsActive is true)
            IsActive = false;
    }
}