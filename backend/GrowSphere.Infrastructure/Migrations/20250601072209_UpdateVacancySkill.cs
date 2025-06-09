using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVacancySkill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "projectVacancyId1",
                table: "projectVacancySkills",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "iX_projectVacancySkills_projectVacancyId1",
                table: "projectVacancySkills",
                column: "projectVacancyId1");

            migrationBuilder.AddForeignKey(
                name: "fK_projectVacancySkills_projectVacancies_projectVacancyId1",
                table: "projectVacancySkills",
                column: "projectVacancyId1",
                principalTable: "projectVacancies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_projectVacancySkills_projectVacancies_projectVacancyId1",
                table: "projectVacancySkills");

            migrationBuilder.DropIndex(
                name: "iX_projectVacancySkills_projectVacancyId1",
                table: "projectVacancySkills");

            migrationBuilder.DropColumn(
                name: "projectVacancyId1",
                table: "projectVacancySkills");
        }
    }
}
