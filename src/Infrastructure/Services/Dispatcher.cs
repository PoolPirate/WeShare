using Common.Services;
using Hangfire;
using MediatR;
using WeShare.Application.Services;

namespace WeShare.Infrastructure.Services;
public class Dispatcher : Singleton, IDispatcher
{
    public void Enqueue(IRequest request, string jobName)
    {
        var client = new BackgroundJobClient();
        client.Enqueue<DispatcherBridge>(x => x.Send(jobName, request));
    }

    public void Schedule(IRequest request, string jobName, TimeSpan delay)
    {
        var client = new BackgroundJobClient();
        client.Schedule<DispatcherBridge>(x => x.Send(jobName, request), delay);
    }
}

public class DispatcherBridge : Scoped
{
    [Inject]
    private readonly IMediator Mediator;

    [JobDisplayName("{0}")]
    public Task Send(string _, IRequest request) => Mediator.Send(request);
}
