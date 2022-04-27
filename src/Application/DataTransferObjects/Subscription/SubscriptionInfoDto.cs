using WeShare.Application.Common.Mappings;
using WeShare.Domain.Entities;

namespace WeShare.Application.DTOs;
public class SubscriptionInfoDto : IMapFrom<Subscription>
{
    public long Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

