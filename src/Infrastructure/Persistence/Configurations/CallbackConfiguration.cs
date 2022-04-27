using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Persistence.Configurations;
public class CallbackConfiguration : IEntityTypeConfiguration<Callback>
{
    public void Configure(EntityTypeBuilder<Callback> builder)
    {
        builder.Property(x => x.Id)
        .ValueGeneratedOnAdd()
        .UseIdentityAlwaysColumn();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Secret);
        builder.HasIndex(x => x.Secret)
        .IsUnique();

        builder.Property(x => x.CreatedAt);
        builder.HasIndex(x => x.CreatedAt);

        builder.Property(x => x.SuccessfullySentAt);

        builder.Property(x => x.Type);

        builder.Property(x => x.OwnerId);

        builder.HasIndex(x => new { x.Type, x.OwnerId })
        .IsUnique();

        builder.ToTable("Callbacks");
    }
}
