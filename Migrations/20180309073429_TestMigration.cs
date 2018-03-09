using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LiturgieMakerAPI.Migrations
{
    public partial class TestMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vers_Lied_LiedId",
                table: "Vers");

            migrationBuilder.AlterColumn<string>(
                name: "Tekst",
                table: "Vers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "LiedId",
                table: "Vers",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vers_Lied_LiedId",
                table: "Vers",
                column: "LiedId",
                principalTable: "Lied",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vers_Lied_LiedId",
                table: "Vers");

            migrationBuilder.AlterColumn<string>(
                name: "Tekst",
                table: "Vers",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<long>(
                name: "LiedId",
                table: "Vers",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Vers_Lied_LiedId",
                table: "Vers",
                column: "LiedId",
                principalTable: "Lied",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
