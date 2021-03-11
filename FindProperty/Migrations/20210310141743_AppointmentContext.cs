using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindProperty.Migrations
{
    public partial class AppointmentContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(nullable: true),
                    property_id = table.Column<int>(nullable: false),
                    appointment_date = table.Column<DateTime>(nullable: false),
                    hour = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.id);
                });

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "Property");

            migrationBuilder.DropTable(
                name: "Agent");
        }
    }
}
