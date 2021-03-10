using Microsoft.EntityFrameworkCore.Migrations;

namespace FindProperty.Migrations.FindProperty1
{
    public partial class AppointmentNewContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "hour",
                table: "Appointment",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Appointment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hour",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Appointment");
        }
    }
}
