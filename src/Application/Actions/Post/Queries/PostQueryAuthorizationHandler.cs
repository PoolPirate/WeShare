using Common.Services;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;
using WeShare.Domain.Entities;

namespace WeShare.Application.Actions.Queries;

[ServiceLevel(1)]
public class PostQueryAuthorizationHandler : AuthorizationHandler<PostId, PostQueryOperation>
{
    [Inject]
    private readonly ICurrentUserService CurrentUserService = null!;

    public override ValueTask<bool> HandleAuthorizationRequestAsync(PostId entity, PostQueryOperation operation, CancellationToken cancellationToken)
    {
        switch (operation)
        {
            case PostQueryOperation.ReadContent:
                return ValueTask.FromResult(true);
        }

        var optionalUserId = CurrentUserService.GetUserId();
        if (!optionalUserId.HasValue)
        {
            return ValueTask.FromResult(false);
        }
        var userId = optionalUserId.Value;

        switch (operation)
        {
            default:
                throw new NotImplementedException(nameof(operation));
        }
    }
}
