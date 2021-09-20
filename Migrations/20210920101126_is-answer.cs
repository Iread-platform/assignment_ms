using Microsoft.EntityFrameworkCore.Migrations;

namespace iread_assignment_ms.Migrations
{
    public partial class isanswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAnswered",
                table: "Answer",
                type: "tinyint(1)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnswered",
                table: "Answer");
        }
    }
}
