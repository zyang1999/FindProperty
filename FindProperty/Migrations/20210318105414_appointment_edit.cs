using Microsoft.EntityFrameworkCore.Migrations;

namespace FindProperty.Migrations
{
    public partial class appointment_edit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Appointment_property_id",
                table: "Appointment",
                column: "property_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Property_property_id",
                table: "Appointment",
                column: "property_id",
                principalTable: "Property",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Property_property_id",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_property_id",
                table: "Appointment");
        }
    }
}
