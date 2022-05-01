using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShare.Infrastructure.Migrations;

public partial class WebhookSubscription : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
        => migrationBuilder.AddColumn<string>(
            name: "TargetUrl",
            table: "Subscriptions",
            type: "text",
            nullable: true);

    protected override void Down(MigrationBuilder migrationBuilder)
        => migrationBuilder.DropColumn(
            name: "TargetUrl",
            table: "Subscriptions");
}
