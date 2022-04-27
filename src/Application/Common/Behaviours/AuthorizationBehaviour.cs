using MediatR;
using System.Reflection;
using WeShare.Application.Common.Security;
using WeShare.Application.Services;

namespace WeShare.Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ICurrentUserService CurrentUserService;

    public AuthorizationBehaviour(ICurrentUserService currentUserService)
    {
        CurrentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var parentType = request.GetType().DeclaringType ?? throw new Exception("Requests must be nested class declarations!");
        var handlerType = parentType.GetNestedTypes()
            .Where(x => x.GetInterfaces().Any(x => x == typeof(IRequestHandler<TRequest, TResponse>)))
            .SingleOrDefault() ?? throw new Exception("Requests must be declared in a class that also contains the handler!");
        var authorizeAttributes = handlerType.GetCustomAttributes<AuthorizeAttribute>();

        if (authorizeAttributes.Any())
        {
            // Must be authenticated user
            if (CurrentUserService.GetUserId() is null)
            {
                throw new UnauthorizedAccessException();
            }
        }

        // User is authorized / authorization not required
        return await next();
    }
}