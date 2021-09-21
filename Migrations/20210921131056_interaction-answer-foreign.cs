using Microsoft.EntityFrameworkCore.Migrations;

namespace iread_assignment_ms.Migrations
{
    public partial class interactionanswerforeign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InteractionQuestionQuestionId",
                table: "Answer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answer_InteractionQuestionQuestionId",
                table: "Answer",
                column: "InteractionQuestionQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Question_InteractionQuestionQuestionId",
                table: "Answer",
                column: "InteractionQuestionQuestionId",
                principalTable: "Question",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Question_InteractionQuestionQuestionId",
                table: "Answer");

            migrationBuilder.DropIndex(
                name: "IX_Answer_InteractionQuestionQuestionId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "InteractionQuestionQuestionId",
                table: "Answer");
        }
    }
}
