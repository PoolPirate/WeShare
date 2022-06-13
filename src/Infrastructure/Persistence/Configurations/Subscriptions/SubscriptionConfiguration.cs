using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Persistence.Configurations;
public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasDiscriminator(x => x.Type)
            .HasValue(SubscriptionType.Dashboard)
            .HasValue<WebhookSubscription>(SubscriptionType.Webhook)
            .HasValue<DiscordSubscription>(SubscriptionType.MessagerDiscord)
            .IsComplete();

        builder.Property(x => x.Id)
        .ValueGeneratedOnAdd()
        .HasValueGenerator<StronglyTypedIdValueGenerator<SubscriptionId>>()
        .UseIdentityAlwaysColumn();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.Type);
        builder.Property(x => x.Name);

        builder.Property(x => x.ShareId);
        builder.Property(x => x.UserId);

        builder.HasMany(x => x.SentPosts)
        .WithOne(x => x.Subscription)
        .HasForeignKey(x => x.SubscriptionId);

        builder.ToTable("Subscriptions");
    }
}
