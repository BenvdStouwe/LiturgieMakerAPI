using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace LiturgieMakerAPI.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Liedbundel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AantalLiederen = table.Column<int>(nullable: false),
                    Naam = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Liedbundel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Liturgie",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Aanvangsdatum = table.Column<DateTime>(nullable: false),
                    Publicatiedatum = table.Column<DateTime>(nullable: false),
                    Titel = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Liturgie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lied",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AantalVerzen = table.Column<int>(nullable: false),
                    LiedNummer = table.Column<int>(nullable: false),
                    LiedbundelId = table.Column<int>(nullable: true),
                    Naam = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lied", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lied_Liedbundel_LiedbundelId",
                        column: x => x.LiedbundelId,
                        principalTable: "Liedbundel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LiturgieItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Discriminator = table.Column<string>(nullable: false),
                    Index = table.Column<int>(nullable: false),
                    LiturgieId = table.Column<long>(nullable: true),
                    LiedId = table.Column<long>(nullable: true),
                    Hoofdstuk = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiturgieItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LiturgieItem_Liturgie_LiturgieId",
                        column: x => x.LiturgieId,
                        principalTable: "Liturgie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LiturgieItem_Lied_LiedId",
                        column: x => x.LiedId,
                        principalTable: "Lied",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LiedId = table.Column<long>(nullable: true),
                    Tekst = table.Column<string>(nullable: true),
                    VersNummer = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vers_Lied_LiedId",
                        column: x => x.LiedId,
                        principalTable: "Lied",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lied_LiedbundelId",
                table: "Lied",
                column: "LiedbundelId");

            migrationBuilder.CreateIndex(
                name: "IX_LiturgieItem_LiturgieId",
                table: "LiturgieItem",
                column: "LiturgieId");

            migrationBuilder.CreateIndex(
                name: "IX_LiturgieItem_LiedId",
                table: "LiturgieItem",
                column: "LiedId");

            migrationBuilder.CreateIndex(
                name: "IX_Vers_LiedId",
                table: "Vers",
                column: "LiedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiturgieItem");

            migrationBuilder.DropTable(
                name: "Vers");

            migrationBuilder.DropTable(
                name: "Liturgie");

            migrationBuilder.DropTable(
                name: "Lied");

            migrationBuilder.DropTable(
                name: "Liedbundel");
        }
    }
}
