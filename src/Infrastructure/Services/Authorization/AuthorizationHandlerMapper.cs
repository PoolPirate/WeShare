using Common.Services;
using System.Reflection;
using WeShare.Application.Common.Security;

namespace WeShare.Infrastructure.Services.Authorization;
public class AuthorizationHandlerMapper : Singleton
{
    private readonly Dictionary<(Type, Type), (Type, Func<object, object, Enum, CancellationToken, ValueTask<bool>>)> AuthorizationHandlerMap;

    public AuthorizationHandlerMapper()
    {
        AuthorizationHandlerMap = new Dictionary<(Type, Type), (Type, Func<object, object, Enum, CancellationToken, ValueTask<bool>>)>();
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

            string? handlerMethodName = nameof(AuthorizationHandler<object, Enum>.HandleAuthorizationRequestAsync);
            var handlerMethod = handlerType.GetMethod(handlerMethodName);

            if (handlerMethod is null)
            {
                throw new MissingMethodException(handlerType.FullName, handlerMethodName);
            }

            AuthorizationHandlerMap.Add((entityType, operationType), (handlerType, HandlerMethodCall));

            ValueTask<bool> HandlerMethodCall(object handler, object entity, Enum operation, CancellationToken cancellationToken)
            {
                return (ValueTask<bool>)handlerMethod!.Invoke(handler,
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

    public (Type, Func<object, object, Enum, CancellationToken, ValueTask<bool>>) GetAuthorizationHandlerType(Type entityType, Type operationType) 
        => AuthorizationHandlerMap.TryGetValue((entityType, operationType), out var handlerType)
            ? handlerType
            : entityType.BaseType is not null &&
              AuthorizationHandlerMap.TryGetValue((entityType.BaseType, operationType), out handlerType)
                ? handlerType
                : throw new InvalidOperationException($"No AuthorizationHandler for entity type {entityType} for operation type {operationType} registered!");
}

