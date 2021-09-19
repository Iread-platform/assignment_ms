using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace iread_assignment_ms.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    AssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<string>(type: "text", nullable: false),
                    TeacherFirstName = table.Column<string>(type: "text", nullable: false),
                    TeacherLastName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.AssignmentId);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentStatus",
                columns: table => new
                {
                    AssignmentStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<string>(type: "text", nullable: true),
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
                    StoryId = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    RightChoiceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Question_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "AssignmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Choice",
                columns: table => new
                {
                    ChoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(type: "text", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: true),
                    MultiChoiceQuestionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choice", x => x.ChoiceId);
                    table.ForeignKey(
                        name: "FK_Choice_Question_MultiChoiceQuestionId",
                        column: x => x.MultiChoiceQuestionId,
                        principalTable: "Question",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Choice_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentStatus_AssignmentId",
                table: "AssignmentStatus",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentStory_AssignmentId",
                table: "AssignmentStory",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Choice_MultiChoiceQuestionId",
                table: "Choice",
                column: "MultiChoiceQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Choice_QuestionId",
                table: "Choice",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_AssignmentId",
                table: "Question",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_RightChoiceId",
                table: "Question",
                column: "RightChoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Choice_RightChoiceId",
                table: "Question",
                column: "RightChoiceId",
                principalTable: "Choice",
                principalColumn: "ChoiceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Assignments_AssignmentId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Choice_Question_MultiChoiceQuestionId",
                table: "Choice");

            migrationBuilder.DropForeignKey(
                name: "FK_Choice_Question_QuestionId",
                table: "Choice");

            migrationBuilder.DropTable(
                name: "AssignmentStatus");

            migrationBuilder.DropTable(
                name: "AssignmentStory");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Choice");
        }
    }
}
