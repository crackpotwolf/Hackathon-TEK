using Microsoft.EntityFrameworkCore.Migrations;

namespace Hackathon_TEK.Migrations
{
    public partial class ReasonAddTypeObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeObject",
                table: "Reasons",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeObject",
                table: "Reasons");
        }
    }
}
