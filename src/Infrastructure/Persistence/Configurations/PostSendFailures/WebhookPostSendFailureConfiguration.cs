using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Persistence.Configurations;
public class WebhookPostSendFailureConfiguration : IEntityTypeConfiguration<WebhookPostSendFailure>
{
    public void Configure(EntityTypeBuilder<WebhookPostSendFailure> builder)
    {
        builder.Property(x => x.StatusCode);
        builder.Property(x => x.ResponseLatency);
    }
}

