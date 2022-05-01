using AutoMapper;
using WeShare.Application.Common.Mappings;
using WeShare.Domain.Entities;

namespace WeShare.Application.DTOs;
public class SentPostInfoDto : IMapFrom<SentPost>
{
    public PostSnippetDto PostSnippet { get; set; } = null!;

    public bool Received { get; set; }
    public DateTimeOffset? ReceivedAt { get; set; }
    public short Attempts { get; set; }

    void IMapFrom<SentPost>.Mapping(Profile profile) 
        => profile.CreateMap<SentPost, SentPostInfoDto>()
            .ForMember(x => x.PostSnippet,
                options => options.MapFrom(s => s.Post));
}

