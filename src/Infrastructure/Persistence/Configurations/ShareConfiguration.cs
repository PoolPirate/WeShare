using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeShare.Domain;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Persistence.Configurations;
public class ShareConfiguration : IEntityTypeConfiguration<Share>
{
    public void Configure(EntityTypeBuilder<Share> builder)
    {
        builder.Property(x => x.Id)
        .ValueGeneratedOnAdd()
        .UseIdentityAlwaysColumn();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.LikeCount);
        builder.Property(x => x.SubscriberCount);

        builder.Property(x => x.Secret);

        builder.Property(x => x.Name)
        .HasMaxLength(DomainConstraints.ShareNameLengthMaximum);
        builder.Property(x => x.Description)
        .HasMaxLength(DomainConstraints.ShareDescriptionLengthMaximum);
        builder.Property(x => x.Readme)
        .HasMaxLength(DomainConstraints.ShareReadmeLengthMaximum);

        builder.Property(x => x.Secret)
        .HasMaxLength(DomainConstraints.ShareSecretLength);

        builder.Property(x => x.HeaderProcessingType);
        builder.Property(x => x.PayloadProcessingType);

        builder.Property(x => x.OwnerId);

        builder.HasMany(x => x.Likes)
        .WithOne(x => x.Share)
        .HasForeignKey(x => x.ShareId);

        builder.HasMany(x => x.Posts)
        .WithOne(x => x.Share)
        .HasForeignKey(x => x.ShareId);

        builder.ToTable("Shares");
    }
}
