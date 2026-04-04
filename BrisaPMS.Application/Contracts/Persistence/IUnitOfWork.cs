namespace BrisaPMS.Application.Contracts.Persistence;

public interface IUnitOfWork
{
    Task Persist();
    Task Revert();
}