using Microsoft.EntityFrameworkCore.Migrations;

namespace iread_assignment_ms.Migrations
{
    public partial class answeressayforeign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EssayAnswerAnswerId",
                table: "Answer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EssayQuestionQuestionId",
                table: "Answer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answer_EssayAnswerAnswerId",
                table: "Answer",
                column: "EssayAnswerAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_EssayQuestionQuestionId",
                table: "Answer",
                column: "EssayQuestionQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Answer_EssayAnswerAnswerId",
                table: "Answer",
                column: "EssayAnswerAnswerId",
                principalTable: "Answer",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Question_EssayQuestionQuestionId",
                table: "Answer",
                column: "EssayQuestionQuestionId",
                principalTable: "Question",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Answer_EssayAnswerAnswerId",
                table: "Answer");

            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Question_EssayQuestionQuestionId",
                table: "Answer");

            migrationBuilder.DropIndex(
                name: "IX_Answer_EssayAnswerAnswerId",
                table: "Answer");

            migrationBuilder.DropIndex(
                name: "IX_Answer_EssayQuestionQuestionId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "EssayAnswerAnswerId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "EssayQuestionQuestionId",
                table: "Answer");
        }
    }
}
