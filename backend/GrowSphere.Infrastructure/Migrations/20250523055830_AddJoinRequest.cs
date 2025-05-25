using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJoinRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "joinRequests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    projectId = table.Column<Guid>(type: "uuid", nullable: false),
                    userId = table.Column<Guid>(type: "uuid", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    managerComment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_joinRequests", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "joinRequests");
        }
    }
}
