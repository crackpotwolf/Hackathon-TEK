using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hackathon_TEK.Migrations
{
    public partial class ChangeFireDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcqTime",
                table: "Fires");

            migrationBuilder.RenameColumn(
                name: "AcqDate",
                table: "Fires",
                newName: "Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Fires",
                newName: "AcqDate");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AcqTime",
                table: "Fires",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
