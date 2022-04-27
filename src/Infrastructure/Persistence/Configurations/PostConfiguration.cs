using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Persistence.Configurations;
public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<StronglyTypedIdValueGenerator<PostId>>()
            .UseIdentityAlwaysColumn();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.HeadersSize);
        builder.Property(x => x.PayloadSize);

        builder.Property(x => x.ShareId);

        builder.ToTable("Posts");
    }
}

