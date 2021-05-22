using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Hackathon_TEK.Migrations
{
    public partial class weatherDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weather");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weather",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CloudsMax = table.Column<double>(type: "double precision", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    HumidityMax = table.Column<double>(type: "double precision", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: false),
                    Percipitation = table.Column<double>(type: "double precision", nullable: true),
                    PressureMax = table.Column<double>(type: "double precision", nullable: true),
                    RegionId = table.Column<int>(type: "integer", nullable: false),
                    StationId = table.Column<int>(type: "integer", nullable: false),
                    StationLat = table.Column<double>(type: "double precision", nullable: false),
                    StationLon = table.Column<double>(type: "double precision", nullable: false),
                    StationName = table.Column<string>(type: "text", nullable: true),
                    StationRegion = table.Column<string>(type: "text", nullable: true),
                    TempAverage = table.Column<double>(type: "double precision", nullable: true),
                    TempAverage0 = table.Column<double>(type: "double precision", nullable: true),
                    TempDifNorm0 = table.Column<double>(type: "double precision", nullable: true),
                    TempMax0 = table.Column<double>(type: "double precision", nullable: true),
                    TempMin0 = table.Column<double>(type: "double precision", nullable: true),
                    WindDegMax = table.Column<double>(type: "double precision", nullable: true),
                    WindSpeedMax = table.Column<double>(type: "double precision", nullable: true)
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
    }
}
