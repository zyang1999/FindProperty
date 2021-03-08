using Microsoft.EntityFrameworkCore.Migrations;

namespace FindProperty.Migrations.FindProperty1
{
    public partial class fee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "imagePath",
                table: "Property",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "fee",
                table: "Property",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "imagePath",
                table: "Property",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "fee",
                table: "Property",
                type: "decimal(18,3)",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
