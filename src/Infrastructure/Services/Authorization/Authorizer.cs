using Common.Services;
using Microsoft.Extensions.DependencyInjection;
using WeShare.Application.Common.Exceptions;
using WeShare.Application.Services;
using WeShare.Infrastructure.Services.Authorization;

namespace WeShare.Infrastructure.Services;
public class Authorizer : Scoped, IAuthorizer
{
    [Inject]
    private readonly IServiceProvider Provider;
    [Inject]
    private readonly ICurrentUserService CurrentUserService;

    [Inject]
    private readonly AuthorizationHandlerMapper AuthorizationHandlerMapper;

    public async ValueTask EnsureAuthorizationAsync(object entity, Enum operation, CancellationToken cancellationToken)
    {
        var handlerDelegates = AuthorizationHandlerMapper.GetAuthorizationHandlerType(entity.GetType(), operation.GetType());

        object handler = Provider.GetRequiredService(handlerDelegates.HandlerType);

        var optionalUserId = CurrentUserService.GetUserId();

        bool authorized = await (optionalUserId.HasValue
            ? handlerDelegates.ExecuteAsync(handler, optionalUserId.Value, entity, operation, cancellationToken)
            : handlerDelegates.ExecuteAsync(handler, entity, operation, cancellationToken));

        if (!authorized)
        {
            throw new ForbiddenAccessException();
        }
    }
}

