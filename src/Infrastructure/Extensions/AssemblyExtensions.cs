using System.Reflection;
using WeShare.Application.Common.Security;

namespace WeShare.Infrastructure.Extensions;
public static partial class Extensions
{
    public static IEnumerable<Type> GetAuthorizationHandlerTypes(this Assembly assembly)
        => assembly.GetTypes()
            .Where(x => x.BaseType == typeof(AuthorizationHandler<,>))
            .Where(x => !x.IsAbstract);

}

