using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace PaintballWorld.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Location : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "Address");

            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "Address",
                type: "geography",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Address");

            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "Address",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
