using AutoMapper;
using System.Net;
using WeShare.Application.Common.Mappings;
using WeShare.Domain.Entities;

namespace WeShare.Application.DTOs;
public class WebhookPostSendFailureDto : PostSendFailureDto, IMapFrom<PostSendFailure>
{
    public HttpStatusCode? StatusCode { get; set; }
    public int ResponseLatency { get; set; }

    public WebhookPostSendFailureDto(HttpStatusCode? statusCode, int responseLatency, DateTimeOffset createdAt)
        : base(createdAt)
    {
        StatusCode = statusCode;
        ResponseLatency = responseLatency;
    }
    public WebhookPostSendFailureDto()
    {
    }

    void IMapFrom<PostSendFailure>.Mapping(Profile profile) //Necessary to override base class definition
        => profile.CreateMap<WebhookPostSendFailure, WebhookPostSendFailureDto>();
}

