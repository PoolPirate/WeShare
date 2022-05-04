using AutoMapper;
using WeShare.Application.Common.Mappings;
using WeShare.Domain.Entities;

namespace WeShare.Application.DTOs;
public class DiscordConnectionSnippetDto : ServiceConnectionSnippetDto, IMapFrom<ServiceConnection>
{
    public ulong DiscordId { get; set; }


    void IMapFrom<ServiceConnection>.Mapping(Profile profile) 
        => profile.CreateMap<DiscordConnection, DiscordConnectionSnippetDto>();
}

