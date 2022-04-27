using WeShare.Application.Common.Mappings;
using WeShare.Domain.Entities;
using WeShare.Domain.Enums;

namespace WeShare.Application.Entities;
public class PostProcessingContext : IMapFrom<Share>
{
    public ShareId ShareId { get; set; }
    public HeaderProcessingType HeaderProcessingType { get; set; }
    public PayloadProcessingType PayloadProcessingType { get; set; }

    void IMapFrom<Share>.Mapping(AutoMapper.Profile profile)
        => profile.CreateMap<Share, PostProcessingContext>()
            .ForMember(x => x.ShareId, options => options.MapFrom(x => x.Id));
}

