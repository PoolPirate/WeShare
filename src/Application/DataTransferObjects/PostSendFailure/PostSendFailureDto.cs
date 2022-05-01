using AutoMapper;
using WeShare.Application.Common.Mappings;
using WeShare.Domain.Entities;

namespace WeShare.Application.DTOs;

public class PostSendFailureDto : IMapFrom<PostSendFailure>
{
    public DateTimeOffset CreatedAt { get; set; }

    public PostSendFailureDto(DateTimeOffset createdAt)
    {
        CreatedAt = createdAt;
    }
    public PostSendFailureDto()
    {
    }

    void IMapFrom<PostSendFailure>.Mapping(Profile profile) => profile.CreateMap<PostSendFailure, PostSendFailureDto>()
            .ConstructUsing((postSendFailure, context)
                => postSendFailure.SubscriptionType switch
                {
                    SubscriptionType.Dashboard => context.Mapper.Map<PostSendFailureDto>(postSendFailure),
                    SubscriptionType.Webhook => context.Mapper.Map<WebhookPostSendFailureDto>((WebhookPostSendFailure)postSendFailure),
                    _ => throw new NotImplementedException(),
                });

}

