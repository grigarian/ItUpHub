using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeJoinRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pK_joinRequests",
                table: "joinRequests");

            migrationBuilder.RenameTable(
                name: "joinRequests",
                newName: "join_requests");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "join_requests",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "projectId",
                table: "join_requests",
                newName: "project_id");

            migrationBuilder.RenameColumn(
                name: "managerComment",
                table: "join_requests",
                newName: "manager_comment");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "join_requests",
                newName: "created_at");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "join_requests",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "message",
                table: "join_requests",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "manager_comment",
                table: "join_requests",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pK_join_requests",
                table: "join_requests",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pK_join_requests",
                table: "join_requests");

            migrationBuilder.RenameTable(
                name: "join_requests",
                newName: "joinRequests");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "joinRequests",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "project_id",
                table: "joinRequests",
                newName: "projectId");

            migrationBuilder.RenameColumn(
                name: "manager_comment",
                table: "joinRequests",
                newName: "managerComment");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "joinRequests",
                newName: "createdAt");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "joinRequests",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "message",
                table: "joinRequests",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "managerComment",
                table: "joinRequests",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pK_joinRequests",
                table: "joinRequests",
                column: "id");
        }
    }
}
