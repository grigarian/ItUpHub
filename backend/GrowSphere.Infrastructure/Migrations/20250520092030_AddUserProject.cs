using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_project_user_project_id",
                table: "project");

            migrationBuilder.DropIndex(
                name: "iX_project_project_id",
                table: "project");

            migrationBuilder.DropColumn(
                name: "project_id",
                table: "project");

            migrationBuilder.CreateTable(
                name: "user_project",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_user_project", x => new { x.user_id, x.project_id });
                    table.ForeignKey(
                        name: "fK_user_project_project_projectId",
                        column: x => x.project_id,
                        principalTable: "project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_user_project_user_userId",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "iX_user_project_projectId",
                table: "user_project",
                column: "project_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_project");

            migrationBuilder.AddColumn<Guid>(
                name: "project_id",
                table: "project",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "iX_project_project_id",
                table: "project",
                column: "project_id");

            migrationBuilder.AddForeignKey(
                name: "fK_project_user_project_id",
                table: "project",
                column: "project_id",
                principalTable: "user",
                principalColumn: "id");
        }
    }
}
