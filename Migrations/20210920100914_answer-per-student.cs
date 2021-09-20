using Microsoft.EntityFrameworkCore.Migrations;

namespace iread_assignment_ms.Migrations
{
    public partial class answerperstudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "Answer",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Answer");
        }
    }
}
