using FluentValidation.Results;

namespace BrisaPMS.Application.Exceptions;

public class ValidationException : Exception
{
    public List<string> ValidationErrors { get; set; } = [];

    public ValidationException(ValidationResult validationResult)
    {
        foreach (var validationError in validationResult.Errors)
        {
            ValidationErrors.Add(validationError.ErrorMessage);
        }
    }
}