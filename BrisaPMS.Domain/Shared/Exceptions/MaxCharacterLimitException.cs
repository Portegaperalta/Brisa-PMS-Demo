using System;

namespace BrisaPMS.Domain.Shared.Exceptions;

public class MaxCharacterLimitException : Exception
{
    public MaxCharacterLimitException(int maxCharacterLimit, string field)
        : base($"The field {field} must have {maxCharacterLimit} characters or less") {}
}