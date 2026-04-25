namespace BrisaPMS.Application.Contracts.Services;

public interface ICurrentUserService
{
    Guid UserId { get; }
}