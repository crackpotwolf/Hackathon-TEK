using Microsoft.EntityFrameworkCore.Migrations;

namespace Hackathon_TEK.Migrations
{
    public partial class ChangeNameAcqTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AcqTimeNew",
                table: "Fires",
                newName: "AcqTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AcqTime",
                table: "Fires",
                newName: "AcqTimeNew");
        }
    }
}
