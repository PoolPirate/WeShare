using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShare.Infrastructure.Migrations;

public partial class PostSendFailureAuditable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "ResponseLatency",
            table: "PostSendFailures",
            type: "integer",
            nullable: true,
            oldClrType: typeof(long),
            oldType: "bigint",
            oldNullable: true);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "CreatedAt",
            table: "PostSendFailures",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "PostSendFailures");

        migrationBuilder.AlterColumn<long>(
            name: "ResponseLatency",
            table: "PostSendFailures",
            type: "bigint",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "integer",
            oldNullable: true);
    }
}
