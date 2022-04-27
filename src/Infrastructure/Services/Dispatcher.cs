using Common.Services;
using Hangfire;
using MediatR;
using WeShare.Application.Services;

namespace WeShare.Infrastructure.Services;
public class Dispatcher : Singleton, IDispatcher
{
    public void Enqueue(IRequest request, string jobName) => BackgroundJob.Enqueue<DispatcherBridge>(x => x.Send(jobName, request));
}

public class DispatcherBridge : Scoped
{
    [Inject]
    private readonly IMediator Mediator;

    [JobDisplayName("{0}")]
    public Task Send(string _, IRequest request) => Mediator.Send(request);
}
