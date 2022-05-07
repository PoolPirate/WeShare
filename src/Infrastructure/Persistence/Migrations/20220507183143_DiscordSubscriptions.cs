using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShare.Infrastructure.Migrations;

public partial class DiscordSubscriptions : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "SubscriptionType",
            table: "PostSendFailures",
            newName: "Type");

        migrationBuilder.AddColumn<string>(
            name: "ChannelId",
            table: "Subscriptions",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsFinal",
            table: "SentPosts",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "IsFinal",
            table: "PostSendFailures",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<int>(
            name: "PublishError",
            table: "PostSendFailures",
            type: "integer",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ChannelId",
            table: "Subscriptions");

        migrationBuilder.DropColumn(
            name: "IsFinal",
            table: "SentPosts");

        migrationBuilder.DropColumn(
            name: "IsFinal",
            table: "PostSendFailures");

        migrationBuilder.DropColumn(
            name: "PublishError",
            table: "PostSendFailures");

        migrationBuilder.RenameColumn(
            name: "Type",
            table: "PostSendFailures",
            newName: "SubscriptionType");
    }
}
