using Microsoft.EntityFrameworkCore.Migrations;

namespace iread_assignment_ms.Migrations
{
    public partial class multichoiceanswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChosenChoiceId",
                table: "Answer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MultiChoiceQuestionId",
                table: "Answer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answer_ChosenChoiceId",
                table: "Answer",
                column: "ChosenChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_MultiChoiceQuestionId",
                table: "Answer",
                column: "MultiChoiceQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Choice_ChosenChoiceId",
                table: "Answer",
                column: "ChosenChoiceId",
                principalTable: "Choice",
                principalColumn: "ChoiceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Question_MultiChoiceQuestionId",
                table: "Answer",
                column: "MultiChoiceQuestionId",
                principalTable: "Question",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Choice_ChosenChoiceId",
                table: "Answer");

            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Question_MultiChoiceQuestionId",
                table: "Answer");

            migrationBuilder.DropIndex(
                name: "IX_Answer_ChosenChoiceId",
                table: "Answer");

            migrationBuilder.DropIndex(
                name: "IX_Answer_MultiChoiceQuestionId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "ChosenChoiceId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "MultiChoiceQuestionId",
                table: "Answer");
        }
    }
}
