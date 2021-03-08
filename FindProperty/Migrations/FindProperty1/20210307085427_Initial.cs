using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindProperty.Migrations.FindProperty1
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Property",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(maxLength: 60, nullable: false),
                    description = table.Column<string>(maxLength: 60, nullable: false),
                    fee = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    size = table.Column<int>(nullable: false),
                    type = table.Column<string>(nullable: false),
                    furnishing = table.Column<string>(nullable: false),
                    address = table.Column<string>(nullable: false),
                    status = table.Column<string>(nullable: true),
                    created_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Property");
        }
    }
}
