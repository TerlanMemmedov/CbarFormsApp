using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CbarFormsApp.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ValTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameForTime = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Valutes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nominal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<double>(type: "float", nullable: true),
                    ValTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Valutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Valutes_ValTypes_ValTypeId",
                        column: x => x.ValTypeId,
                        principalTable: "ValTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Valutes_ValTypeId",
                table: "Valutes",
                column: "ValTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Valutes");

            migrationBuilder.DropTable(
                name: "ValTypes");
        }
    }
}
