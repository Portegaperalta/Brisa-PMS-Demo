namespace BrisaPMS.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, Guid id) : base($"{entityName} with id {id} not found") {}
}