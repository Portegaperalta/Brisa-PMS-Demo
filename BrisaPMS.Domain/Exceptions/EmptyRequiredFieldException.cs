using System;

namespace BrisaPMS.Domain.Exceptions
{
    public class EmptyRequiredFieldException(string fieldName) : Exception($"The field {fieldName} cannot be empty")
    {
    }
}
