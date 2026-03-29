using BrisaPMS.Domain.Enums;

namespace BrisaPMS.Domain.Entities;

public class Role
{
    public Guid Id { get; private set; }
    public RoleType Type { get; private set; }
}