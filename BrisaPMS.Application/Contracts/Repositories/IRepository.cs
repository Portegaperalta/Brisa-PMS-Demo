namespace BrisaPMS.Application.Contracts.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetById(Guid id);
    Task<IEnumerable<T>> GetAll();
    Task<T> Create(T entity);
    Task Update(T entity);
    Task Delete(T entity);
    Task<bool> Exists(Guid id);
}