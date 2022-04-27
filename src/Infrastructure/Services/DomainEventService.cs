using Common.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using WeShare.Application.Common.Models;
using WeShare.Application.Services;
using WeShare.Domain.Common;

namespace WeShare.Infrastructure.Services;

public class DomainEventService : Scoped, IDomainEventService
{
    [Inject]
    private readonly IPublisher Mediator;

    [Inject]
    private readonly ILogger<DomainEventService> Logger;

    public async Task Publish(DomainEvent domainEvent)
    {
        Logger.LogInformation("Publishing domain event. Event - {event}", domainEvent.GetType().Name);
        await Mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));
    }

    private static INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent) => (INotification)Activator.CreateInstance(
            typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent)!;
}
