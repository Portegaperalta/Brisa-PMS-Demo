using System;

namespace BrisaPMS.Domain.Shared.Exceptions
{
    public class EmptyRequiredFieldException(string fieldName) : Exception($"The field {fieldName} cannot be empty")
    {
    }
}
