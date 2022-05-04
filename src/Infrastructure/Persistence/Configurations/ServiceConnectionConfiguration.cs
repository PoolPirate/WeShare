using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Persistence.Configurations;
public class ServiceConnectionConfiguration : IEntityTypeConfiguration<ServiceConnection>
{
    public void Configure(EntityTypeBuilder<ServiceConnection> builder)
    {
        builder.HasDiscriminator(x => x.Type)
            .HasValue(ServiceConnectionType.None)
            .HasValue<DiscordConnection>(ServiceConnectionType.Discord)
            .IsComplete();

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<StronglyTypedIdValueGenerator<ServiceConnectionId>>()
            .UseIdentityAlwaysColumn();

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.UserId);

        builder.ToTable("ServiceConnections");
    }
}

