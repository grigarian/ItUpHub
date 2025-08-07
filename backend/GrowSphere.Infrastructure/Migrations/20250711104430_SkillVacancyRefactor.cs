using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SkillVacancyRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "projectVacancySkills");

            migrationBuilder.CreateTable(
                name: "projectVacancySkill",
                columns: table => new
                {
                    projectVacancyId = table.Column<Guid>(type: "uuid", nullable: false),
                    skillsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_projectVacancySkill", x => new { x.projectVacancyId, x.skillsId });
                    table.ForeignKey(
                        name: "fK_projectVacancySkill_projectVacancies_projectVacancyId",
                        column: x => x.projectVacancyId,
                        principalTable: "projectVacancies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_projectVacancySkill_skill_skillsId",
                        column: x => x.skillsId,
                        principalTable: "skill",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "iX_projectVacancySkill_skillsId",
                table: "projectVacancySkill",
                column: "skillsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "projectVacancySkill");

            migrationBuilder.CreateTable(
                name: "projectVacancySkills",
                columns: table => new
                {
                    projectVacancyId = table.Column<Guid>(type: "uuid", nullable: false),
                    skillId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_projectVacancySkills", x => new { x.projectVacancyId, x.skillId });
                    table.ForeignKey(
                        name: "fK_projectVacancySkills_projectVacancies_projectVacancyId",
                        column: x => x.projectVacancyId,
                        principalTable: "projectVacancies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_projectVacancySkills_skill_skillId",
                        column: x => x.skillId,
                        principalTable: "skill",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "iX_projectVacancySkills_skillId",
                table: "projectVacancySkills",
                column: "skillId");
        }
    }
}
