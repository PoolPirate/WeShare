using Common.Services;
using MediatR;
using WeShare.Application.Actions.Tasks;
using WeShare.Application.Common.Models;
using WeShare.Application.Services;
using WeShare.Domain.Entities;
using WeShare.Domain.Events;

namespace WeShare.Application.Actions.EventHandlers;
public class Created : INotificationHandler<DomainEventNotification<PostCreatedEvent>>
{
    private readonly IDispatcher Dispatcher;

    public Created(IDispatcher dispatcher)
    {
        Dispatcher = dispatcher;
    }

    public Task Handle(DomainEventNotification<PostCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var post = notification.DomainEvent.Post;
        Dispatcher.Enqueue(new PostPublishToWebhookTask.Command(post.Id),
            $"Webhook Publish Post {post.Id}");

        return Task.CompletedTask;
    }
}

