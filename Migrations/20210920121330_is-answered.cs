using Microsoft.EntityFrameworkCore.Migrations;

namespace iread_assignment_ms.Migrations
{
    public partial class isanswered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Answer_EssayAnswerAnswerId",
                table: "Answer");

            migrationBuilder.DropIndex(
                name: "IX_Answer_EssayAnswerAnswerId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "EssayAnswerAnswerId",
                table: "Answer");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAnswered",
                table: "Answer",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsAnswered",
                table: "Answer",
                type: "tinyint(1)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AddColumn<int>(
                name: "EssayAnswerAnswerId",
                table: "Answer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answer_EssayAnswerAnswerId",
                table: "Answer",
                column: "EssayAnswerAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Answer_EssayAnswerAnswerId",
                table: "Answer",
                column: "EssayAnswerAnswerId",
                principalTable: "Answer",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
