using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Persistence.Configurations;
public class PostSendFailureConfiguration : IEntityTypeConfiguration<PostSendFailure>
{
    public void Configure(EntityTypeBuilder<PostSendFailure> builder)
    {
        builder.HasDiscriminator(x => x.Type)
            .HasValue(PostSendFailureType.InternalError)
            .HasValue<WebhookPostSendFailure>(PostSendFailureType.Webhook)
            .HasValue<DiscordPostSendFailure>(PostSendFailureType.MessagerDiscord);

        builder.Property(x => x.Id);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.PostId);
        builder.Property(x => x.SubscriptionId);

        builder.Property(x => x.Type);

        builder.ToTable("PostSendFailures");
    }
}

