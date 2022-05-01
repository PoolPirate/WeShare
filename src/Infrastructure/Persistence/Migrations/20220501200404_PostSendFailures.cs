using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeShare.Infrastructure.Migrations;

public partial class PostSendFailures : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
        => migrationBuilder.CreateTable(
            name: "PostSendFailures",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                PostId = table.Column<long>(type: "bigint", nullable: false),
                SubscriptionId = table.Column<long>(type: "bigint", nullable: false),
                SubscriptionType = table.Column<int>(type: "integer", nullable: false),
                StatusCode = table.Column<int>(type: "integer", nullable: true),
                ResponseLatency = table.Column<long>(type: "bigint", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_PostSendFailures", x => x.Id));

    protected override void Down(MigrationBuilder migrationBuilder)
        => migrationBuilder.DropTable(
            name: "PostSendFailures");
}
