using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Persistence.Configurations;
public class DiscordSubscriptionConfiguration : IEntityTypeConfiguration<DiscordSubscription>
{
    public void Configure(EntityTypeBuilder<DiscordSubscription> builder)
        => builder.Property(x => x.ChannelId);
}

