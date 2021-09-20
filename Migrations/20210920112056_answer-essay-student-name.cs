using Microsoft.EntityFrameworkCore.Migrations;

namespace iread_assignment_ms.Migrations
{
    public partial class answeressaystudentname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudentFirstName",
                table: "Answer",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "StudentLastName",
                table: "Answer",
                type: "text",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentFirstName",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "StudentLastName",
                table: "Answer");
        }
    }
}
