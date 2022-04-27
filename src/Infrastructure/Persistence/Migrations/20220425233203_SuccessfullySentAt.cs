using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WeShare.Infrastructure.Migrations;

public partial class SuccessfullySentAt : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_Callbacks",
            table: "Callbacks");

        migrationBuilder.AlterColumn<string>(
            name: "Nickname",
            table: "Users",
            type: "character varying(20)",
            maxLength: 20,
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "character varying(20)",
            oldMaxLength: 20,
            oldNullable: true);

        migrationBuilder.AddColumn<long>(
            name: "Id",
            table: "Callbacks",
            type: "bigint",
            nullable: false,
            defaultValue: 0L)
            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

        migrationBuilder.AddColumn<DateTimeOffset>(
            name: "SuccessfullySentAt",
            table: "Callbacks",
            type: "timestamp with time zone",
            nullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_Callbacks",
            table: "Callbacks",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_Callbacks_Secret",
            table: "Callbacks",
            column: "Secret",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_Callbacks",
            table: "Callbacks");

        migrationBuilder.DropIndex(
            name: "IX_Callbacks_Secret",
            table: "Callbacks");

        migrationBuilder.DropColumn(
            name: "Id",
            table: "Callbacks");

        migrationBuilder.DropColumn(
            name: "SuccessfullySentAt",
            table: "Callbacks");

        migrationBuilder.AlterColumn<string>(
            name: "Nickname",
            table: "Users",
            type: "character varying(20)",
            maxLength: 20,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "character varying(20)",
            oldMaxLength: 20);

        migrationBuilder.AddPrimaryKey(
            name: "PK_Callbacks",
            table: "Callbacks",
            column: "Secret");
    }
}
