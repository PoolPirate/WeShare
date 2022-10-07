using AutoMapper;
using WeShare.Application.Common.Mappings;
using WeShare.Domain.Entities;

namespace WeShare.Application.DTOs;

public class PostSendFailureDto : IMapFrom<PostSendFailure>
{
    public PostSendFailureType Type { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public PostSendFailureDto(PostSendFailureType type, DateTimeOffset createdAt)
    {
        Type = type;
        CreatedAt = createdAt;
    }
    public PostSendFailureDto()
    {
    }

    void IMapFrom<PostSendFailure>.Mapping(Profile profile) => profile.CreateMap<PostSendFailure, PostSendFailureDto>()
            .ConstructUsing((postSendFailure, context)
                => postSendFailure.Type switch
                {
                    PostSendFailureType.InternalError => new PostSendFailureDto(postSendFailure.Type, postSendFailure.CreatedAt),
                    PostSendFailureType.Webhook => context.Mapper.Map<WebhookPostSendFailureDto>((WebhookPostSendFailure)postSendFailure),
                    PostSendFailureType.MessagerDiscord => context.Mapper.Map<DiscordPostSendFailureDto>((DiscordPostSendFailure)postSendFailure),
                    _ => throw new NotImplementedException(),
                });

}

