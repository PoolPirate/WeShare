using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShare.Infrastructure.Migrations;

public partial class Relation : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_PostSendFailures_PostId_SubscriptionId",
            table: "PostSendFailures",
            columns: new[] { "PostId", "SubscriptionId" });

        migrationBuilder.AddForeignKey(
            name: "FK_PostSendFailures_SentPosts_PostId_SubscriptionId",
            table: "PostSendFailures",
            columns: new[] { "PostId", "SubscriptionId" },
            principalTable: "SentPosts",
            principalColumns: new[] { "PostId", "SubscriptionId" },
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_PostSendFailures_SentPosts_PostId_SubscriptionId",
            table: "PostSendFailures");

        migrationBuilder.DropIndex(
            name: "IX_PostSendFailures_PostId_SubscriptionId",
            table: "PostSendFailures");
    }
}
