using MediatR;
using WeShare.Application.Common.Models;
using WeShare.Domain.Events;

namespace WeShare.Application.Actions.EventHandlers;
public class Created : INotificationHandler<DomainEventNotification<PostCreatedEvent>>
{
    public Task Handle(DomainEventNotification<PostCreatedEvent> notification, CancellationToken cancellationToken) => Task.CompletedTask;
}

