using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserSkill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_skill_user_user_id",
                table: "skill");

            migrationBuilder.DropIndex(
                name: "iX_skill_user_id",
                table: "skill");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "skill");

            migrationBuilder.CreateTable(
                name: "user_skill",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    skill_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_user_skill", x => new { x.user_id, x.skill_id });
                    table.ForeignKey(
                        name: "fK_user_skill_skill_skillId",
                        column: x => x.skill_id,
                        principalTable: "skill",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_user_skill_user_userId",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "iX_user_skill_skillId",
                table: "user_skill",
                column: "skill_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_skill");

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "skill",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "iX_skill_user_id",
                table: "skill",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fK_skill_user_user_id",
                table: "skill",
                column: "user_id",
                principalTable: "user",
                principalColumn: "id");
        }
    }
}
