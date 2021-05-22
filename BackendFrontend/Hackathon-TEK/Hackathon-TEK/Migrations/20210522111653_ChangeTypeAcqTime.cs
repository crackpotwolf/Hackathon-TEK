using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hackathon_TEK.Migrations
{
    public partial class ChangeTypeAcqTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcqTime",
                table: "Fires");

            migrationBuilder.DropColumn(
                name: "Bright_t31",
                table: "Fires");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AcqTimeNew",
                table: "Fires",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<double>(
                name: "BrightT31",
                table: "Fires",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcqTimeNew",
                table: "Fires");

            migrationBuilder.DropColumn(
                name: "BrightT31",
                table: "Fires");

            migrationBuilder.AddColumn<DateTime>(
                name: "AcqTime",
                table: "Fires",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Bright_t31",
                table: "Fires",
                type: "text",
                nullable: true);
        }
    }
}
