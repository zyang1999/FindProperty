using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindProperty.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agent",
                columns: table => new
                {
                    AgentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: false),
                    phone_number = table.Column<string>(nullable: false),
                    profile_picture = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agent", x => x.AgentID);
                });

            migrationBuilder.CreateTable(
                name: "Property",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(maxLength: 60, nullable: false),
                    description = table.Column<string>(nullable: false),
                    fee = table.Column<int>(nullable: false),
                    size = table.Column<int>(nullable: false),
                    type = table.Column<string>(nullable: false),
                    property_type = table.Column<string>(nullable: false),
                    furnishing = table.Column<string>(nullable: false),
                    address = table.Column<string>(nullable: false),
                    imagePath = table.Column<string>(nullable: true),
                    AgentID = table.Column<int>(nullable: false),
                    created_at = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => x.id);
                    table.ForeignKey(
                        name: "FK_Property_Agent_AgentID",
                        column: x => x.AgentID,
                        principalTable: "Agent",
                        principalColumn: "AgentID",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    table.ForeignKey(
                        name: "FK_Appointment_Property_property_id",
                        column: x => x.property_id,
                        principalTable: "Property",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_property_id",
                table: "Appointment",
                column: "property_id");

            migrationBuilder.CreateIndex(
                name: "IX_Property_AgentID",
                table: "Property",
                column: "AgentID");
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
