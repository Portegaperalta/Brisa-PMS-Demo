namespace BrisaPMS.Domain.Shared.Abstractions;

public abstract class BaseEntity
{
    public Guid CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid? UpdatedBy { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    internal void SetAuditFields(Guid createdBy, DateTime createdAt)
    {
        CreatedBy = createdBy;
        CreatedAt = createdAt;
    }

    internal void SetUpdateFields(Guid updatedBy, DateTime updatedAt)
    {
        UpdatedBy = updatedBy;
        UpdatedAt = updatedAt;
    }
}