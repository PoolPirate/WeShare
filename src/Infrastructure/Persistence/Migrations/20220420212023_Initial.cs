using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WeShare.Infrastructure.Migrations;

public partial class Initial : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                Username = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                EmailVerified = table.Column<bool>(type: "boolean", nullable: false),
                PasswordHash = table.Column<string>(type: "text", nullable: false),
                Nickname = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                LikesPublished = table.Column<bool>(type: "boolean", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Users", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Callbacks",
            columns: table => new
            {
                Secret = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                Type = table.Column<int>(type: "integer", nullable: false),
                OwnerId = table.Column<long>(type: "bigint", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Callbacks", x => x.Secret);
                table.ForeignKey(
                    name: "FK_Callbacks_Users_OwnerId",
                    column: x => x.OwnerId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Shares",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                LikeCount = table.Column<int>(type: "integer", nullable: false),
                SubscriberCount = table.Column<int>(type: "integer", nullable: false),
                Secret = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                Readme = table.Column<string>(type: "character varying(4096)", maxLength: 4096, nullable: false),
                HeaderProcessingType = table.Column<int>(type: "integer", nullable: false),
                BodyProcessingType = table.Column<int>(type: "integer", nullable: false),
                OwnerId = table.Column<long>(type: "bigint", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Shares", x => x.Id);
                table.ForeignKey(
                    name: "FK_Shares_Users_OwnerId",
                    column: x => x.OwnerId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Likes",
            columns: table => new
            {
                OwnerId = table.Column<long>(type: "bigint", nullable: false),
                ShareId = table.Column<long>(type: "bigint", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Likes", x => new { x.OwnerId, x.ShareId });
                table.ForeignKey(
                    name: "FK_Likes_Shares_ShareId",
                    column: x => x.ShareId,
                    principalTable: "Shares",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Likes_Users_OwnerId",
                    column: x => x.OwnerId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Callbacks_CreatedAt",
            table: "Callbacks",
            column: "CreatedAt");

        migrationBuilder.CreateIndex(
            name: "IX_Callbacks_OwnerId",
            table: "Callbacks",
            column: "OwnerId");

        migrationBuilder.CreateIndex(
            name: "IX_Callbacks_Type_OwnerId",
            table: "Callbacks",
            columns: new[] { "Type", "OwnerId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Likes_ShareId",
            table: "Likes",
            column: "ShareId");

        migrationBuilder.CreateIndex(
            name: "IX_Shares_OwnerId",
            table: "Shares",
            column: "OwnerId");

        migrationBuilder.CreateIndex(
            name: "IX_Users_Email",
            table: "Users",
            column: "Email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Users_Username",
            table: "Users",
            column: "Username",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Callbacks");

        migrationBuilder.DropTable(
            name: "Likes");

        migrationBuilder.DropTable(
            name: "Shares");

        migrationBuilder.DropTable(
            name: "Users");
    }
}
