using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintballWorld.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class teraz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "FieldSchedule",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FieldScheduleId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_FieldScheduleId",
                table: "Events",
                column: "FieldScheduleId",
                unique: true,
                filter: "[FieldScheduleId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_FieldSchedule_FieldScheduleId",
                table: "Events",
                column: "FieldScheduleId",
                principalTable: "FieldSchedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_FieldSchedule_FieldScheduleId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_FieldScheduleId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "FieldSchedule");

            migrationBuilder.DropColumn(
                name: "FieldScheduleId",
                table: "Events");
        }
    }
}
