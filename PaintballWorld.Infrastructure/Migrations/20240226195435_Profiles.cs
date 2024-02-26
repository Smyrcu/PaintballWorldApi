using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaintballWorld.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Profiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_Field_FieldId",
                table: "Photo");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "UserInfo");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "UserInfo",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileImageId",
                table: "UserInfo",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MainPhotoId",
                table: "Field",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_UserInfo_ProfileImageId",
                table: "UserInfo",
                column: "ProfileImageId",
                unique: true,
                filter: "[ProfileImageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Field_MainPhotoId",
                table: "Field",
                column: "MainPhotoId",
                unique: true,
                filter: "[MainPhotoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Field_Photo_MainPhotoId",
                table: "Field",
                column: "MainPhotoId",
                principalTable: "Photo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_Field_FieldId",
                table: "Photo",
                column: "FieldId",
                principalTable: "Field",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInfo_Photo_ProfileImageId",
                table: "UserInfo",
                column: "ProfileImageId",
                principalTable: "Photo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Field_Photo_MainPhotoId",
                table: "Field");

            migrationBuilder.DropForeignKey(
                name: "FK_Photo_Field_FieldId",
                table: "Photo");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInfo_Photo_ProfileImageId",
                table: "UserInfo");

            migrationBuilder.DropIndex(
                name: "IX_UserInfo_ProfileImageId",
                table: "UserInfo");

            migrationBuilder.DropIndex(
                name: "IX_Field_MainPhotoId",
                table: "Field");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "UserInfo");

            migrationBuilder.DropColumn(
                name: "ProfileImageId",
                table: "UserInfo");

            migrationBuilder.DropColumn(
                name: "MainPhotoId",
                table: "Field");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "UserInfo",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_Field_FieldId",
                table: "Photo",
                column: "FieldId",
                principalTable: "Field",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
