using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShare.Infrastructure.Migrations;

public partial class DiscordIdIndex : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
        => migrationBuilder.CreateIndex(
            name: "IX_ServiceConnections_DiscordId",
            table: "ServiceConnections",
            column: "DiscordId",
            unique: true);

    protected override void Down(MigrationBuilder migrationBuilder)
        => migrationBuilder.DropIndex(
            name: "IX_ServiceConnections_DiscordId",
            table: "ServiceConnections");
}
