using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WeShare.Infrastructure.Migrations;

public partial class Posts : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "BodyProcessingType",
            table: "Shares",
            newName: "PayloadProcessingType");

        migrationBuilder.AlterColumn<string>(
            name: "Secret",
            table: "Callbacks",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(32)",
            oldMaxLength: 32);

        migrationBuilder.CreateTable(
            name: "Posts",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                HeadersSize = table.Column<long>(type: "bigint", nullable: false),
                PayloadSize = table.Column<long>(type: "bigint", nullable: false),
                ShareId = table.Column<long>(type: "bigint", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Posts", x => x.Id);
                table.ForeignKey(
                    name: "FK_Posts_Shares_ShareId",
                    column: x => x.ShareId,
                    principalTable: "Shares",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Posts_ShareId",
            table: "Posts",
            column: "ShareId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Posts");

        migrationBuilder.RenameColumn(
            name: "PayloadProcessingType",
            table: "Shares",
            newName: "BodyProcessingType");

        migrationBuilder.AlterColumn<string>(
            name: "Secret",
            table: "Callbacks",
            type: "character varying(32)",
            maxLength: 32,
            nullable: false,
            oldClrType: typeof(string),
            oldType: "text");
    }
}
