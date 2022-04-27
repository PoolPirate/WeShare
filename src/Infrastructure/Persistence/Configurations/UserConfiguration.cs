using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeShare.Domain;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Persistence.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Ignore(x => x.DomainEvents);

        builder.Property(x => x.Id)
        .ValueGeneratedOnAdd()
        .HasValueGenerator<StronglyTypedIdValueGenerator<UserId>>()
        .UseIdentityAlwaysColumn();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.Username)
        .HasMaxLength(DomainConstraints.UsernameLengthMaximum);
        builder.HasIndex(x => x.Username)
        .IsUnique();

        builder.Property(x => x.Email)
        .HasMaxLength(DomainConstraints.EmailLengthMaximum);
        builder.HasIndex(x => x.Email)
        .IsUnique();
        builder.Property(x => x.EmailVerified);
        builder.Property(x => x.PasswordHash);

        builder.Property(x => x.Nickname)
        .HasMaxLength(DomainConstraints.NicknameLengthMaximum);

        builder.HasMany(x => x.Shares)
        .WithOne(x => x.Owner)
        .HasForeignKey(x => x.OwnerId);

        builder.HasMany(x => x.Likes)
        .WithOne(x => x.Owner)
        .HasForeignKey(x => x.OwnerId);

        builder.HasMany(x => x.Callbacks)
        .WithOne(x => x.Owner)
        .HasForeignKey(x => x.OwnerId);

        builder.ToTable("Users");
    }
}
