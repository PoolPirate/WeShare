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
    private readonly AuthorizationHandlerMapper AuthorizationHandlerMapper;

    public async ValueTask EnsureAuthorizationAsync(object entity, Enum operation, CancellationToken cancellation)
    {
        var (handlerType, handlerDelegate) = AuthorizationHandlerMapper.GetAuthorizationHandlerType(entity.GetType(), operation.GetType());
        object? handler = Provider.GetRequiredService(handlerType);
        bool authorized = await handlerDelegate.Invoke(handler, entity, operation, cancellation);

        if (!authorized)
        {
            throw new ForbiddenAccessException();
        }
    }
}

