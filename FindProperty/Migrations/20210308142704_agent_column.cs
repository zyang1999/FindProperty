using Microsoft.EntityFrameworkCore.Migrations;

namespace FindProperty.Migrations
{
    public partial class agent_column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "agent_name",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Property");

            migrationBuilder.AddColumn<string>(
                name: "agent_id",
                table: "Property",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "agent_id",
                table: "Property");

            migrationBuilder.AddColumn<string>(
                name: "agent_name",
                table: "Property",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "Property",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Property",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
