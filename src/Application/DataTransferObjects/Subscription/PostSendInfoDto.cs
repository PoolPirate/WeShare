using AutoMapper;
using System;
using System.Threading.Tasks;
using WeShare.Application.Common.Mappings;
using WeShare.Domain.Entities;

namespace WeShare.Application.DTOs;
public class PostSendInfoDto : IMapFrom<SentPost>
{
    public PostSnippetDto PostSnippet { get; set; } = null!;
    //Explicit object type for serializer to detect the derived types
    public List<object> PostSendFailures { get; set; } = null!;

    public bool Received { get; set; }
    public DateTimeOffset? ReceivedAt { get; set; }
    public short Attempts { get; set; }

    void IMapFrom<SentPost>.Mapping(Profile profile) 
        => profile.CreateMap<SentPost, PostSendInfoDto>()
            .ForMember(x => x.PostSnippet, options => options.MapFrom(x => x.Post))
            .ForMember(x => x.PostSendFailures, options => options.Ignore());
}

