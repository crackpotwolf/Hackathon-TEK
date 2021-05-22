using Microsoft.EntityFrameworkCore.Migrations;

namespace Hackathon_TEK.Migrations
{
    public partial class regionUpdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "StationLon",
                table: "Weather",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StationLon",
                table: "Weather");
        }
    }
}
