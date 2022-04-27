using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WeShare.Application.Common.Models;
using WeShare.Application.Services;
using WeShare.Domain.Enums;
using WeShare.Domain.Events;

namespace WeShare.Application.Actions.EventHandlers;
public class UserEmailUpdatedEventHandler : INotificationHandler<DomainEventNotification<UserEmailUpdatedEvent>>
{
    private readonly IShareContext DbContext;
    private readonly ICallbackService CallbackService;
    private readonly ILogger Logger;

    public UserEmailUpdatedEventHandler(IShareContext dbContext, ICallbackService callbackService, ILogger<UserEmailUpdatedEventHandler> logger)
    {
        DbContext = dbContext;
        CallbackService = callbackService;
        Logger = logger;
    }

    public async Task Handle(DomainEventNotification<UserEmailUpdatedEvent> notification, CancellationToken cancellationToken)
    {
        var userId = notification.DomainEvent.UserId;

        var existingCallback = await DbContext.Callbacks
            .Where(x => x.OwnerId == userId && x.Type == CallbackType.EmailVerification)
            .SingleOrDefaultAsync(cancellationToken: cancellationToken);

        if (existingCallback is not null)
        {
            Logger.LogWarning("There already is a valid verification callback, replacing it with new one");
            DbContext.Callbacks.Remove(existingCallback);
        }

        var callback = CallbackService.Create(userId, CallbackType.EmailVerification);
        DbContext.Callbacks.Add(callback);

        await DbContext.SaveChangesAsync(cancellationToken: cancellationToken);
    }
}

