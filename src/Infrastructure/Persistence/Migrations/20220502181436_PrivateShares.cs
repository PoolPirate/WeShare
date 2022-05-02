using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShare.Infrastructure.Migrations;

public partial class PrivateShares : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder) 
        => migrationBuilder.AddColumn<bool>(
            name: "IsPrivate",
            table: "Shares",
            type: "boolean",
            nullable: false,
            defaultValue: false);

    protected override void Down(MigrationBuilder migrationBuilder) 
        => migrationBuilder.DropColumn(
            name: "IsPrivate",
            table: "Shares");
}
