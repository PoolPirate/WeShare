using MediatR;
using Microsoft.Extensions.Logging;
using WeShare.Application.Common.Exceptions;

namespace WeShare.Application.Common.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> Logger;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
    {
        Logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (Exception ex) when (ex is not ForbiddenAccessException && ex is not UnauthorizedAccessException)
        {
            string? requestName = typeof(TRequest).Name;

            Logger.LogError(ex, "CleanArchitecture Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

            throw;
        }
    }
}
