using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_issue_project_projectId",
                table: "issue");

            migrationBuilder.RenameColumn(
                name: "projectId",
                table: "issue",
                newName: "project_id");

            migrationBuilder.RenameIndex(
                name: "iX_issue_projectId",
                table: "issue",
                newName: "iX_issue_project_id");

            migrationBuilder.AddForeignKey(
                name: "fK_issue_project_project_id",
                table: "issue",
                column: "project_id",
                principalTable: "project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_issue_project_project_id",
                table: "issue");

            migrationBuilder.RenameColumn(
                name: "project_id",
                table: "issue",
                newName: "projectId");

            migrationBuilder.RenameIndex(
                name: "iX_issue_project_id",
                table: "issue",
                newName: "iX_issue_projectId");

            migrationBuilder.AddForeignKey(
                name: "fK_issue_project_projectId",
                table: "issue",
                column: "projectId",
                principalTable: "project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
