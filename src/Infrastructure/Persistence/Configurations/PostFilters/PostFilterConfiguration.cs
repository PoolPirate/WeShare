using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeShare.Domain;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Persistence.Configurations;

public class PostFilterConfiguration : IEntityTypeConfiguration<PostFilter>
{
    public void Configure(EntityTypeBuilder<PostFilter> builder)
    {
        builder.HasDiscriminator(x => x.Type)
            .HasValue<JsonSchemaPostFilter>(PostFilterType.JsonSchema)
            .IsComplete();

        builder.Property(x => x.Id)
            .HasValueGenerator<StronglyTypedIdValueGenerator<PostFilterId>>()
            .ValueGeneratedOnAdd()
            .UseIdentityAlwaysColumn();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt);

        builder.Property(x => x.Name)
            .HasMaxLength(DomainConstraints.ShareFilterNameLengthMaximum);

        builder.Property(x => x.ShareId);

        builder.ToTable("PostFilters");
    }
}
