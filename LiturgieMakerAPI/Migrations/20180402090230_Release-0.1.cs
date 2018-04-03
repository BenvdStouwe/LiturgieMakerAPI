using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LiturgieMakerAPI.Migrations
{
    public partial class Release01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lied_Liedbundel_LiedbundelId",
                table: "Lied");

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Liturgie",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "LiedbundelId",
                table: "Lied",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lied_Liedbundel_LiedbundelId",
                table: "Lied",
                column: "LiedbundelId",
                principalTable: "Liedbundel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lied_Liedbundel_LiedbundelId",
                table: "Lied");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Liturgie");

            migrationBuilder.AlterColumn<int>(
                name: "LiedbundelId",
                table: "Lied",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Lied_Liedbundel_LiedbundelId",
                table: "Lied",
                column: "LiedbundelId",
                principalTable: "Liedbundel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
