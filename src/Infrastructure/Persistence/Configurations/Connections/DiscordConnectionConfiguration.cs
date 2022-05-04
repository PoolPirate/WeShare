using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Persistence.Configurations;
public class DiscordConnectionConfiguration : IEntityTypeConfiguration<DiscordConnection>
{
    public void Configure(EntityTypeBuilder<DiscordConnection> builder)
    {
        builder.Property(x => x.DiscordId);
    
        builder.HasIndex(x => x.DiscordId)
            .IsUnique();
    }
}

