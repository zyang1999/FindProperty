using Microsoft.EntityFrameworkCore.Migrations;

namespace FindProperty.Migrations.FindProperty1
{
    public partial class AgentContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "Property");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "Property",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AddColumn<int>(
                name: "AgentID",
                table: "Property",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "property_type",
                table: "Property",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.CreateIndex(
                name: "IX_Property_AgentID",
                table: "Property",
                column: "AgentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Property_Agent_AgentID",
                table: "Property",
                column: "AgentID",
                principalTable: "Agent",
                principalColumn: "AgentID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Property_Agent_AgentID",
                table: "Property");

            migrationBuilder.DropTable(
                name: "Agent");

            migrationBuilder.DropIndex(
                name: "IX_Property_AgentID",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "AgentID",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "property_type",
                table: "Property");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "Property",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Property",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
