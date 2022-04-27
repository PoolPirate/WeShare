using WeShare.Application.Common.Mappings;
using WeShare.Domain.Entities;
using WeShare.Domain.Enums;

namespace WeShare.Application.DTOs;

public class ShareSecretsDto : IMapFrom<Share>
{
    public string Secret { get; private set; } = null!;
    public HeaderProcessingType HeaderProcessingType { get; private set; }
    public PayloadProcessingType PayloadProcessingType { get; private set; }
}
