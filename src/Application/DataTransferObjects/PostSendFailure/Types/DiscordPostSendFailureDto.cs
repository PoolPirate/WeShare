using AutoMapper;
using WeShare.Application.Common.Mappings;
using WeShare.Domain.Entities;

namespace WeShare.Application.DTOs;
public class DiscordPostSendFailureDto : PostSendFailureDto, IMapFrom<PostSendFailure>
{
    public DiscordPublishError PublishError { get; private set; }

    public DiscordPostSendFailureDto(DiscordPublishError publishError, DateTimeOffset createdAt)
        : base(PostSendFailureType.Webhook, createdAt)
    {
        PublishError = publishError;
    }
    public DiscordPostSendFailureDto()
    {
    }

    void IMapFrom<PostSendFailure>.Mapping(Profile profile) //Necessary to override base class definition
        => profile.CreateMap<DiscordPostSendFailure, DiscordPostSendFailureDto>();
}

