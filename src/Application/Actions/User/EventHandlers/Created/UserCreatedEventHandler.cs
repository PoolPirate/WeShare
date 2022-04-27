using MediatR;
using WeShare.Application.Common.Models;
using WeShare.Application.Services;
using WeShare.Domain.Enums;
using WeShare.Domain.Events;

namespace WeShare.Application.Actions.EventHandlers;
public class UserCreatedHandler : INotificationHandler<DomainEventNotification<UserCreatedEvent>>
{
    private readonly IShareContext DbContext;
    private readonly ICallbackService CallbackService;

    public UserCreatedHandler(IShareContext dbContext, ICallbackService callbackService)
    {
        DbContext = dbContext;
        CallbackService = callbackService;
    }

    public async Task Handle(DomainEventNotification<UserCreatedEvent> notification, CancellationToken cancellationToken)
    {
        var userId = notification.DomainEvent.User.Id;
        var callback = CallbackService.Create(userId, CallbackType.EmailVerification);
        DbContext.Callbacks.Add(callback);

        await DbContext.SaveChangesAsync(cancellationToken: cancellationToken);
    }
}
