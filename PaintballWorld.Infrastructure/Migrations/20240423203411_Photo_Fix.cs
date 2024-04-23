using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintballWorld.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Photo_Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_Field_FieldId",
                table: "Photo");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_Field_FieldId",
                table: "Photo",
                column: "FieldId",
                principalTable: "Field",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_Field_FieldId",
                table: "Photo");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_Field_FieldId",
                table: "Photo",
                column: "FieldId",
                principalTable: "Field",
                principalColumn: "Id");
        }
    }
}
