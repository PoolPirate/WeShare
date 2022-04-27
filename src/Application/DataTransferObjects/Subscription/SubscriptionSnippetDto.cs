using WeShare.Application.Common.Mappings;
using WeShare.Domain.Entities;

namespace WeShare.Application.DTOs;
public class SubscriptionSnippetDto : IMapFrom<Subscription>
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
}

