using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Services.Authorization;
public class AuthenticationHandlerDelegates
{
    public Type HandlerType { get; }

    private readonly Func<object, UserId, object, Enum, CancellationToken, ValueTask<bool>> AuthenticatedDelegate;
    private readonly Func<object, object, Enum, CancellationToken, ValueTask<bool>> UnauthenticatedDelegate;

    public AuthenticationHandlerDelegates(Type handlerType,
        Func<object, UserId, object, Enum, CancellationToken, ValueTask<bool>> authenticatedDelegate,
        Func<object, object, Enum, CancellationToken, ValueTask<bool>> unauthenticatedDelegate)
    {
        HandlerType = handlerType;

        AuthenticatedDelegate = authenticatedDelegate;
        UnauthenticatedDelegate = unauthenticatedDelegate;
    }

    public ValueTask<bool> ExecuteAsync(object handler, UserId userId, object entity, Enum operation, CancellationToken cancellationToken)
        => AuthenticatedDelegate.Invoke(handler, userId, entity, operation, cancellationToken);

    public ValueTask<bool> ExecuteAsync(object handler, object entity, Enum operation, CancellationToken cancellationToken)
    => UnauthenticatedDelegate.Invoke(handler, entity, operation, cancellationToken);
}

