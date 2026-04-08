using BrisaPMS.Application.Exceptions;
using FluentValidation;
using ValidationException = BrisaPMS.Application.Exceptions.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace BrisaPMS.Application.Utilities.Mediator;

public class SimpleMediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;
    
    public SimpleMediator(IServiceProvider serviceProvider) { _serviceProvider = serviceProvider; }
    
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        var validatorType = typeof(IValidator<>).MakeGenericType(request.GetType());
        
        var validator = _serviceProvider.GetService(validatorType);

        if (validator is not null)
        {
            var validateMethod = validatorType.GetMethod("ValidateAsync");
            var validateTask = (Task)validateMethod!.Invoke(validator, 
                    new object[] { request, CancellationToken.None })!;
            
            await  validateTask.ConfigureAwait(false);

            var result = validateTask.GetType().GetProperty("Result");
            var validationResult = (ValidationResult)result!.GetValue(validateTask)!;
            
            if (validationResult.IsValid is not true)
                throw new ValidationException(validationResult);
        }
        
        var useCaseType = typeof(IRequestHandler<,>)
            .MakeGenericType(request.GetType(), typeof(TResponse));
        
        var useCase = _serviceProvider.GetService(useCaseType);

        if (useCase is null)
            throw new MediatorException($"Handler not found for {request.GetType().Name}");

        var method = useCaseType.GetMethod("Handle")!;
        
        return await (Task<TResponse>)method.Invoke(useCase, new object[] { request })!;
    }
}