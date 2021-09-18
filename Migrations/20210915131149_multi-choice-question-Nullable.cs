using Microsoft.EntityFrameworkCore.Migrations;

namespace iread_assignment_ms.Migrations
{
    public partial class multichoicequestionNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Choice_RightChoiceId",
                table: "Question");

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
                name: "FK_Question_Choice_RightChoiceId",
                table: "Question");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Choice_RightChoiceId",
                table: "Question",
                column: "RightChoiceId",
                principalTable: "Choice",
                principalColumn: "ChoiceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
