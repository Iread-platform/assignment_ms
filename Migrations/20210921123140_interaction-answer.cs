using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace iread_assignment_ms.Migrations
{
    public partial class interactionanswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnswerInteraction",
                columns: table => new
                {
                    AnswerInteractionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    InteractionId = table.Column<int>(type: "int", nullable: true),
                    InteractionAnswerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerInteraction", x => x.AnswerInteractionId);
                    table.ForeignKey(
                        name: "FK_AnswerInteraction_Answer_InteractionAnswerId",
                        column: x => x.InteractionAnswerId,
                        principalTable: "Answer",
                        principalColumn: "AnswerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerInteraction_InteractionAnswerId",
                table: "AnswerInteraction",
                column: "InteractionAnswerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerInteraction");
        }
    }
}
