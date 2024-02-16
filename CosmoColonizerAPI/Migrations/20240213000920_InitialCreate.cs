using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CosmoColonizerAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Planets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Temperature = table.Column<double>(type: "float", nullable: true),
                    OxygenVolume = table.Column<double>(type: "float", nullable: true),
                    WaterVolume = table.Column<double>(type: "float", nullable: true),
                    Gravity = table.Column<double>(type: "float", nullable: true),
                    AtmosphericPressure = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlanetId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Planets_PlanetId",
                        column: x => x.PlanetId,
                        principalTable: "Planets",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Planets",
                columns: new[] { "Id", "AtmosphericPressure", "Gravity", "ImageUrl", "Name", "OxygenVolume", "Temperature", "WaterVolume" },
                values: new object[,]
                {
                    { 1, null, null, null, "Terra Nova", null, null, null },
                    { 2, null, null, null, "Luna Magna", null, null, null },
                    { 3, null, null, null, "Solara", null, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_PlanetId",
                table: "Users",
                column: "PlanetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Planets");
        }
    }
}
