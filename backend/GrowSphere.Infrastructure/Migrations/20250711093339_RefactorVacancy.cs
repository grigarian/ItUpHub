using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrowSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorVacancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "join_requests");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "projectVacancies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "iX_vacancyApplications_userId_projectVacancyId",
                table: "vacancyApplications",
                columns: new[] { "userId", "projectVacancyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "iX_projectVacancies_projectId",
                table: "projectVacancies",
                column: "projectId");

            migrationBuilder.AddForeignKey(
                name: "fK_projectVacancies_project_projectId",
                table: "projectVacancies",
                column: "projectId",
                principalTable: "project",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fK_vacancyApplications_user_userId",
                table: "vacancyApplications",
                column: "userId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_projectVacancies_project_projectId",
                table: "projectVacancies");

            migrationBuilder.DropForeignKey(
                name: "fK_vacancyApplications_user_userId",
                table: "vacancyApplications");

            migrationBuilder.DropIndex(
                name: "iX_vacancyApplications_userId_projectVacancyId",
                table: "vacancyApplications");

            migrationBuilder.DropIndex(
                name: "iX_projectVacancies_projectId",
                table: "projectVacancies");

            migrationBuilder.DropColumn(
                name: "status",
                table: "projectVacancies");

            migrationBuilder.CreateTable(
                name: "join_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    manager_comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_join_requests", x => x.id);
                });
        }
    }
}
