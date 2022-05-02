using Common.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WeShare.Application.Common.Security;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Services.Authorization;
public class AuthorizationHandlerMapper : Singleton
{
    private readonly Dictionary<(Type, Type), AuthenticationHandlerDelegates> AuthorizationHandlerMap;

    public AuthorizationHandlerMapper()
    {
        AuthorizationHandlerMap = new Dictionary<(Type, Type), AuthenticationHandlerDelegates>();
    }

    protected override ValueTask InitializeAsync()
    {
        var baseType = typeof(AuthorizationHandler<,>);

        var handlerTypes = Assembly.GetAssembly(baseType)!
            .GetTypes()
            .Where(x => !x.IsAbstract)
            .Where(x => x.BaseType is not null)
            .Where(x => !x.BaseType!.IsGenericTypeDefinition &&
                        x.BaseType.IsGenericType &&
                        x.BaseType.GetGenericTypeDefinition() == baseType);

        foreach (var handlerType in handlerTypes)
        {
            var entityType = handlerType.BaseType!.GenericTypeArguments[0];
            var operationType = handlerType.BaseType!.GenericTypeArguments[1];

            string? authenticatedHandlerMethodName = nameof(AuthorizationHandler<object, Enum>.HandleAuthenticatedRequestAsync);
            var authenticatedHandlerMethod = handlerType.GetMethod(authenticatedHandlerMethodName);

            if (authenticatedHandlerMethod is null)
            {
                throw new MissingMethodException(handlerType.FullName, authenticatedHandlerMethodName);
            }

            string? unauthenticatedHandlerMethodName = nameof(AuthorizationHandler<object, Enum>.HandleUnauthenticatedRequestAsync);
            var unauthenticatedHandlerMethod = handlerType.GetMethod(unauthenticatedHandlerMethodName);

            if (unauthenticatedHandlerMethod is null)
            {
                throw new MissingMethodException(handlerType.FullName, unauthenticatedHandlerMethodName);
            }

            var handlerDelegates = new AuthenticationHandlerDelegates(handlerType,
                (handler, userId, entity, operation, cancellationToken) => AuthenticatedHandlerMethodCall(handler, userId, entity, operation, cancellationToken),
                (handler, entity, operation, cancellationToken) => UnauthenticatedHandlerMethodCall(handler, entity, operation, cancellationToken));

            AuthorizationHandlerMap.Add((entityType, operationType), handlerDelegates);

            ValueTask<bool> AuthenticatedHandlerMethodCall(object handler, UserId userId, object entity, Enum operation, CancellationToken cancellationToken)
            {
                return (ValueTask<bool>)authenticatedHandlerMethod!.Invoke(handler,
                    new object[]
                    {
                        userId,
                        entity,
                        operation,
                        cancellationToken
                    })!;
            }
            ValueTask<bool> UnauthenticatedHandlerMethodCall(object handler, object entity, Enum operation, CancellationToken cancellationToken)
            {
                return (ValueTask<bool>)unauthenticatedHandlerMethod!.Invoke(handler,
                    new object[]
                    {
                        entity,
                        operation,
                        cancellationToken
                    })!;
            }
        }

        return base.InitializeAsync();
    }

    public AuthenticationHandlerDelegates GetAuthorizationHandlerType(Type entityType, Type operationType)
        => AuthorizationHandlerMap.TryGetValue((entityType, operationType), out var handlerType)
            ? handlerType
            : entityType.BaseType is not null &&
              AuthorizationHandlerMap.TryGetValue((entityType.BaseType, operationType), out handlerType)
                ? handlerType
                : throw new InvalidOperationException($"No AuthorizationHandler for entity type {entityType} for operation type {operationType} registered!");
}

