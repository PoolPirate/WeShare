using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShare.Infrastructure.Migrations;

public partial class SubscriptionName : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Name",
            table: "Subscriptions",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateIndex(
            name: "IX_Subscriptions_LastReceivedPostId",
            table: "Subscriptions",
            column: "LastReceivedPostId");

        migrationBuilder.AddForeignKey(
            name: "FK_Subscriptions_Posts_LastReceivedPostId",
            table: "Subscriptions",
            column: "LastReceivedPostId",
            principalTable: "Posts",
            principalColumn: "Id");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Subscriptions_Posts_LastReceivedPostId",
            table: "Subscriptions");

        migrationBuilder.DropIndex(
            name: "IX_Subscriptions_LastReceivedPostId",
            table: "Subscriptions");

        migrationBuilder.DropColumn(
            name: "Name",
            table: "Subscriptions");
    }
}
