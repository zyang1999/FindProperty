using Microsoft.EntityFrameworkCore.Migrations;

namespace FindProperty.Migrations
{
    public partial class agent_foreign_key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Property_Agent_AgentId",
                table: "Property");

            migrationBuilder.RenameColumn(
                name: "AgentId",
                table: "Property",
                newName: "AgentID");

            migrationBuilder.RenameIndex(
                name: "IX_Property_AgentId",
                table: "Property",
                newName: "IX_Property_AgentID");

            migrationBuilder.RenameColumn(
                name: "AgentId",
                table: "Agent",
                newName: "AgentID");

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                table: "Agent",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Agent",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

            migrationBuilder.RenameColumn(
                name: "AgentID",
                table: "Property",
                newName: "AgentId");

            migrationBuilder.RenameIndex(
                name: "IX_Property_AgentID",
                table: "Property",
                newName: "IX_Property_AgentId");

            migrationBuilder.RenameColumn(
                name: "AgentID",
                table: "Agent",
                newName: "AgentId");

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                table: "Agent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Agent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Property_Agent_AgentId",
                table: "Property",
                column: "AgentId",
                principalTable: "Agent",
                principalColumn: "AgentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
