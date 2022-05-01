using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Persistence.Configurations;
public class WebhookSubscriptionConfiguration : IEntityTypeConfiguration<WebhookSubscription>
{
    public void Configure(EntityTypeBuilder<WebhookSubscription> builder)
        => builder.Property(x => x.TargetUrl);
}

