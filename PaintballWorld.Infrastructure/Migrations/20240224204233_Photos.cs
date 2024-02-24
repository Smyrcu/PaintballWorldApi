using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintballWorld.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Photos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_EntityType_EntityTypeId",
                table: "Photo");

            migrationBuilder.DropIndex(
                name: "IX_Photo_EntityTypeId",
                table: "Photo");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Photo_FieldId_EventId",
                table: "Photo");

            migrationBuilder.DropColumn(
                name: "EntityTypeId",
                table: "Photo");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Photo",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Photo");

            migrationBuilder.AddColumn<Guid>(
                name: "EntityTypeId",
                table: "Photo",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Photo_EntityTypeId",
                table: "Photo",
                column: "EntityTypeId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Photo_FieldId_EventId",
                table: "Photo",
                sql: "([FieldId] IS NOT NULL AND [EventId] IS NULL) OR ([FieldId] IS NULL AND [EventId] IS NOT NULL)");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_EntityType_EntityTypeId",
                table: "Photo",
                column: "EntityTypeId",
                principalTable: "EntityType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
