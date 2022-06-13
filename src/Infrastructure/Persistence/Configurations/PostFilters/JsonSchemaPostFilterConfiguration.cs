using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeShare.Domain.Entities;

namespace WeShare.Infrastructure.Persistence.Configurations;

public class JsonSchemaPostFilterConfiguration : IEntityTypeConfiguration<JsonSchemaPostFilter>
{
    public void Configure(EntityTypeBuilder<JsonSchemaPostFilter> builder) 
        => builder.Property(x => x.Schema);
}
