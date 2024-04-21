using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintballWorld.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Poprawki_Bazy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_FieldTypeName_FieldTypeId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "FieldTypeId",
                table: "Events",
                newName: "FieldId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_FieldTypeId",
                table: "Events",
                newName: "IX_Events_FieldId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Field_FieldId",
                table: "Events",
                column: "FieldId",
                principalTable: "Field",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Field_FieldId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "FieldId",
                table: "Events",
                newName: "FieldTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_FieldId",
                table: "Events",
                newName: "IX_Events_FieldTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_FieldTypeName_FieldTypeId",
                table: "Events",
                column: "FieldTypeId",
                principalTable: "FieldTypeName",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
