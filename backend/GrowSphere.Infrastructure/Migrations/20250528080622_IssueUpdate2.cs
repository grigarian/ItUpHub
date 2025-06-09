using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IssueUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_issue_user_assignerUserId",
                table: "issue");

            migrationBuilder.RenameColumn(
                name: "assignerUserId",
                table: "issue",
                newName: "assigner_user_id");

            migrationBuilder.RenameIndex(
                name: "iX_issue_assignerUserId",
                table: "issue",
                newName: "iX_issue_assigner_user_id");

            migrationBuilder.AddColumn<Guid>(
                name: "assigned_to_user_id",
                table: "issue",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "order",
                table: "issue",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "iX_issue_assigned_to_user_id",
                table: "issue",
                column: "assigned_to_user_id");

            migrationBuilder.AddForeignKey(
                name: "fK_issue_user_assigned_to_user_id",
                table: "issue",
                column: "assigned_to_user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fK_issue_user_assigner_user_id",
                table: "issue",
                column: "assigner_user_id",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_issue_user_assigned_to_user_id",
                table: "issue");

            migrationBuilder.DropForeignKey(
                name: "fK_issue_user_assigner_user_id",
                table: "issue");

            migrationBuilder.DropIndex(
                name: "iX_issue_assigned_to_user_id",
                table: "issue");

            migrationBuilder.DropColumn(
                name: "assigned_to_user_id",
                table: "issue");

            migrationBuilder.DropColumn(
                name: "order",
                table: "issue");

            migrationBuilder.RenameColumn(
                name: "assigner_user_id",
                table: "issue",
                newName: "assignerUserId");

            migrationBuilder.RenameIndex(
                name: "iX_issue_assigner_user_id",
                table: "issue",
                newName: "iX_issue_assignerUserId");

            migrationBuilder.AddForeignKey(
                name: "fK_issue_user_assignerUserId",
                table: "issue",
                column: "assignerUserId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
