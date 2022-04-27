using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Persistence.Configurations;
public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.Property(x => x.Id)
        .ValueGeneratedOnAdd()
        .HasValueGenerator<StronglyTypedIdValueGenerator<SubscriptionId>>()
        .UseIdentityAlwaysColumn();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.Type);

        builder.Property(x => x.ShareId);
        builder.Property(x => x.UserId);
        builder.Property(x => x.LastReceivedPostId);

        builder.HasOne(x => x.LastReceivedPost);

        builder.ToTable("Subscriptions");
    }
}
