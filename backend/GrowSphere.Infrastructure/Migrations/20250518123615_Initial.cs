using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GrowSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    bio = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    mime_type = table.Column<string>(type: "text", nullable: true),
                    path = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "project",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    categoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    update_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    project_id = table.Column<Guid>(type: "uuid", nullable: true),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_project", x => x.id);
                    table.ForeignKey(
                        name: "fK_project_category_categoryId",
                        column: x => x.categoryId,
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_project_user_project_id",
                        column: x => x.project_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "skill",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_skill", x => x.id);
                    table.ForeignKey(
                        name: "fK_skill_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "issue",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    projectId = table.Column<Guid>(type: "uuid", nullable: false),
                    assignerUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    complete_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    mime_type = table.Column<string>(type: "text", nullable: true),
                    picture_path = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_issue", x => x.id);
                    table.ForeignKey(
                        name: "fK_issue_project_projectId",
                        column: x => x.projectId,
                        principalTable: "project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_issue_user_assignerUserId",
                        column: x => x.assignerUserId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "member",
                columns: table => new
                {
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_member", x => new { x.project_id, x.id });
                    table.ForeignKey(
                        name: "fK_member_project_project_id",
                        column: x => x.project_id,
                        principalTable: "project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_member_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "iX_issue_assignerUserId",
                table: "issue",
                column: "assignerUserId");

            migrationBuilder.CreateIndex(
                name: "iX_issue_projectId",
                table: "issue",
                column: "projectId");

            migrationBuilder.CreateIndex(
                name: "iX_member_user_id",
                table: "member",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "iX_project_categoryId",
                table: "project",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "iX_project_project_id",
                table: "project",
                column: "project_id");

            migrationBuilder.CreateIndex(
                name: "iX_skill_user_id",
                table: "skill",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "issue");

            migrationBuilder.DropTable(
                name: "member");

            migrationBuilder.DropTable(
                name: "skill");

            migrationBuilder.DropTable(
                name: "project");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
