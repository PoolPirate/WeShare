using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Persistence.Configurations;
public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.Property(x => x.OwnerId);
        builder.Property(x => x.ShareId);
        builder.HasKey(x => new { x.OwnerId, x.ShareId });

        builder.Property(x => x.CreatedAt);

        builder.ToTable("Likes");
    }
}
