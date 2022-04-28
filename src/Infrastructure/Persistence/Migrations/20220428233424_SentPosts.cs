using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShare.Infrastructure.Migrations;

public partial class SentPosts : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Subscriptions_Posts_LastReceivedPostId",
            table: "Subscriptions");

        migrationBuilder.DropIndex(
            name: "IX_Subscriptions_LastReceivedPostId",
            table: "Subscriptions");

        migrationBuilder.DropColumn(
            name: "LastReceivedPostId",
            table: "Subscriptions");

        migrationBuilder.CreateTable(
            name: "SentPosts",
            columns: table => new
            {
                PostId = table.Column<long>(type: "bigint", nullable: false),
                SubscriptionId = table.Column<long>(type: "bigint", nullable: false),
                Received = table.Column<bool>(type: "boolean", nullable: false),
                ReceivedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                Attempts = table.Column<short>(type: "smallint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SentPosts", x => new { x.PostId, x.SubscriptionId });
                table.ForeignKey(
                    name: "FK_SentPosts_Posts_PostId",
                    column: x => x.PostId,
                    principalTable: "Posts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_SentPosts_Subscriptions_SubscriptionId",
                    column: x => x.SubscriptionId,
                    principalTable: "Subscriptions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_SentPosts_SubscriptionId",
            table: "SentPosts",
            column: "SubscriptionId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SentPosts");

        migrationBuilder.AddColumn<long>(
            name: "LastReceivedPostId",
            table: "Subscriptions",
            type: "bigint",
            nullable: true);

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
}
