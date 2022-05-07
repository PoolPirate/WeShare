using AutoMapper;
using WeShare.Application.Common.Mappings;
using WeShare.Domain.Entities;

namespace WeShare.Application.DTOs;
public class ServiceConnectionSnippetDto : IMapFrom<ServiceConnection>
{
    public long Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public ServiceConnectionType Type { get; set; }

    void IMapFrom<ServiceConnection>.Mapping(Profile profile)
        => profile.CreateMap<ServiceConnection, ServiceConnectionSnippetDto>()
            .ConstructUsing((serviceConnection, context)
                => serviceConnection.Type switch
                {
                    ServiceConnectionType.None => throw new InvalidOperationException(),
                    ServiceConnectionType.Discord => context.Mapper.Map<DiscordConnectionSnippetDto>((DiscordConnection)serviceConnection),
                    _ => throw new NotImplementedException(),
                });
}

