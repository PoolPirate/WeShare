using Common.Services;
using Microsoft.EntityFrameworkCore;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Commands;

[ServiceLevel(1)]
public class ServiceConnectionAuthorizationHandler : AuthorizationHandler<ServiceConnection, ServiceConnectionCommandOperation>
{
    [Inject]
    private readonly IShareContext DbContext = null!;

    public override async ValueTask<bool> HandleAuthenticatedRequestAsync(UserId authenticatedUser, ServiceConnection entity, ServiceConnectionCommandOperation operation,
        CancellationToken cancellationToken = default)
        => operation switch
        {
            ServiceConnectionCommandOperation.Remove 
                => await DbContext.ServiceConnections
                    .Where(x => x.Id == entity.Id)
                    .AllAsync(x => x.UserId == authenticatedUser, cancellationToken),
            _ => throw new InvalidOperationException(),
        };
    public override ValueTask<bool> HandleUnauthenticatedRequestAsync(ServiceConnection entity, ServiceConnectionCommandOperation operation,
        CancellationToken cancellationToken = default)
        => ValueTask.FromResult(false);
}
