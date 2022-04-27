using MediatR;
using WeShare.Application.Actions.Commands;
using WeShare.Application.Common.Models;
using WeShare.Application.Services;
using WeShare.Domain.Enums;
using WeShare.Domain.Events;

namespace WeShare.Application.Actions.EventHandlers;
public class CallbackCreatedEventHandler : INotificationHandler<DomainEventNotification<CallbackCreatedEvent>>
{
    private readonly IDispatcher Dispatcher;

    public CallbackCreatedEventHandler(IDispatcher dispatcher)
    {
        Dispatcher = dispatcher;
    }

    public Task Handle(DomainEventNotification<CallbackCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var callback = notification.DomainEvent.Callback;

        switch (callback.Type)
        {
            case CallbackType.EmailVerification:
                Dispatcher.Enqueue(new SendEmailVerificationAction.Command(callback.Id), $"Send Verification To User {callback.OwnerId}");
                return Task.CompletedTask;
            case CallbackType.PasswordReset:
                Dispatcher.Enqueue(new SendEmailPasswordResetAction.Command(callback.Id), $"Send Password Reset To User {callback.OwnerId}");
                return Task.CompletedTask;
            default:
                return Task.CompletedTask;
        }
    }
}

