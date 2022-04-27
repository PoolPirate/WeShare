using AutoMapper;
using WeShare.Application.Common.Mappings;
using WeShare.Domain.Entities;

namespace WeShare.Application.DTOs;

public class ShareSnippetDto : IMapFrom<Share>
{
    public long Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    public string OwnerUsername { get; private set; } = null!;

    void IMapFrom<Share>.Mapping(Profile profile) => profile.CreateMap<Share, ShareSnippetDto>()
            .ForMember(x => x.OwnerUsername, member => member.MapFrom(share => share.Owner!.Username));
}
