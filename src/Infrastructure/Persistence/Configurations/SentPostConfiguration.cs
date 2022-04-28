using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Persistence.Configurations;
public class SentPostConfiguration : IEntityTypeConfiguration<SentPost>
{
    public void Configure(EntityTypeBuilder<SentPost> builder)
    {
        builder.Property(x => x.PostId);
        builder.Property(x => x.SubscriptionId);

        builder.HasKey(x => new {x.PostId, x.SubscriptionId});

        builder.Property(x => x.Received);
        builder.Property(x => x.ReceivedAt);

        builder.Property(x => x.Attempts);

        builder.ToTable("SentPosts");
    }
}

