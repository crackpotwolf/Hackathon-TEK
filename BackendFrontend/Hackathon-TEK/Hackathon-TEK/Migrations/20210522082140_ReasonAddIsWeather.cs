using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Hackathon_TEK.Migrations
{
    public partial class ReasonAddIsWeather : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWeather",
                table: "Reasons",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Weather",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StationId = table.Column<int>(type: "integer", nullable: false),
                    StationName = table.Column<string>(type: "text", nullable: true),
                    StationRegion = table.Column<string>(type: "text", nullable: true),
                    StationLat = table.Column<double>(type: "double precision", nullable: false),
                    StationLon = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    TempMin0 = table.Column<double>(type: "double precision", nullable: false),
                    TempAverage0 = table.Column<double>(type: "double precision", nullable: false),
                    TempMax0 = table.Column<double>(type: "double precision", nullable: false),
                    TempDifNorm0 = table.Column<double>(type: "double precision", nullable: false),
                    Percipitation = table.Column<double>(type: "double precision", nullable: false),
                    TempAverage = table.Column<double>(type: "double precision", nullable: false),
                    PressureMax = table.Column<double>(type: "double precision", nullable: false),
                    HumidityMax = table.Column<double>(type: "double precision", nullable: false),
                    WindSpeedMax = table.Column<double>(type: "double precision", nullable: false),
                    WindDegMax = table.Column<double>(type: "double precision", nullable: false),
                    CloudsMax = table.Column<double>(type: "double precision", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    RegionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weather", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Weather_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weather_RegionId",
                table: "Weather",
                column: "RegionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weather");

            migrationBuilder.DropColumn(
                name: "IsWeather",
                table: "Reasons");
        }
    }
}
