using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WeShare.Infrastructure.Migrations;

public partial class ServiceConnections : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
        => migrationBuilder.CreateTable(
            name: "ServiceConnections",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                UserId = table.Column<long>(type: "bigint", nullable: false),
                Type = table.Column<int>(type: "integer", nullable: false),
                DiscordId = table.Column<string>(type: "text", nullable: true),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_ServiceConnections", x => x.Id));

    protected override void Down(MigrationBuilder migrationBuilder)
        => migrationBuilder.DropTable(
            name: "ServiceConnections");
}
