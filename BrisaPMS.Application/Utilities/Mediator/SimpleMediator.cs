using BrisaPMS.Application.Exceptions;

namespace BrisaPMS.Application.Utilities.Mediator;

public class SimpleMediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;
    
    public SimpleMediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        var useCaseType = typeof(IRequestHandler<,>)
            .MakeGenericType(request.GetType(), typeof(TResponse));
        
        var useCase = _serviceProvider.GetService(useCaseType);

        if (useCase is null)
            throw new MediatorException($"Handler not found for {request.GetType().Name}");

        var method = useCaseType.GetMethod("Handle")!;
        
        return await (Task<TResponse>)method.Invoke(useCase, new object[] { request })!;
    }
}