using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVacancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "projectVacancies",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    projectId = table.Column<Guid>(type: "uuid", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_projectVacancies", x => x.id);
                });

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

            migrationBuilder.CreateTable(
                name: "vacancyApplications",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    projectVacancyId = table.Column<Guid>(type: "uuid", nullable: false),
                    userId = table.Column<Guid>(type: "uuid", nullable: false),
                    message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    managerComment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_vacancyApplications", x => x.id);
                    table.ForeignKey(
                        name: "fK_vacancyApplications_projectVacancies_projectVacancyId",
                        column: x => x.projectVacancyId,
                        principalTable: "projectVacancies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "iX_projectVacancySkills_skillId",
                table: "projectVacancySkills",
                column: "skillId");

            migrationBuilder.CreateIndex(
                name: "iX_vacancyApplications_projectVacancyId",
                table: "vacancyApplications",
                column: "projectVacancyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "projectVacancySkills");

            migrationBuilder.DropTable(
                name: "vacancyApplications");

            migrationBuilder.DropTable(
                name: "projectVacancies");
        }
    }
}
