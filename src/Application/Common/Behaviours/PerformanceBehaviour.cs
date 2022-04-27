using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace WeShare.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch Timer;
    private readonly ILogger<TRequest> Logger;

    public PerformanceBehaviour(
        ILogger<TRequest> logger)
    {
        Timer = new Stopwatch();

        Logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        Timer.Start();

        var response = await next();

        Timer.Stop();

        long elapsedMilliseconds = Timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            string? requestName = typeof(TRequest).Name;

            Logger.LogWarning("CleanArchitecture Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
                requestName, elapsedMilliseconds, request);
        }

        return response;
    }
}
