using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace iread_assignment_ms.Migrations
{
    public partial class storystatusassignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Assignments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Assignments",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Assignments",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TeacherFirstName",
                table: "Assignments",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "Assignments",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "TeacherLastName",
                table: "Assignments",
                type: "text",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "AssignmentStatus",
                columns: table => new
                {
                    AssignmentStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    StudentFirstName = table.Column<string>(type: "text", nullable: false),
                    StudentLastName = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentStatus", x => x.AssignmentStatusId);
                    table.ForeignKey(
                        name: "FK_AssignmentStatus_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "AssignmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentStory",
                columns: table => new
                {
                    AssignmentStoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    StorytId = table.Column<int>(type: "int", nullable: false),
                    StoryTitle = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentStory", x => x.AssignmentStoryId);
                    table.ForeignKey(
                        name: "FK_AssignmentStory_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "AssignmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentStatus_AssignmentId",
                table: "AssignmentStatus",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentStory_AssignmentId",
                table: "AssignmentStory",
                column: "AssignmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentStatus");

            migrationBuilder.DropTable(
                name: "AssignmentStory");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "TeacherFirstName",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "TeacherLastName",
                table: "Assignments");
        }
    }
}
