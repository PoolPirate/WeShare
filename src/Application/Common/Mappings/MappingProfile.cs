using AutoMapper;
using System.Reflection;
using WeShare.Domain.Entities;

namespace WeShare.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());

        CreateMap<UserId, long>()
            .ConvertUsing(x => x.Value);

        CreateMap<ShareId, long>()
            .ConvertUsing(x => x.Value);

        CreateMap<CallbackId, long>()
            .ConvertUsing(x => x.Value);

        CreateMap<SubscriptionId, long>()
            .ConvertUsing(x => x.Value);
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        foreach (var type in types)
        {
            object? instance = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod(nameof(IMapFrom<object>.Mapping))
                ?? type.GetInterface("IMapFrom`1")!.GetMethod(nameof(IMapFrom<object>.Mapping));

            methodInfo?.Invoke(instance, new object[] { this });
        }
    }
}
