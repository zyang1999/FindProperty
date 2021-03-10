using Microsoft.EntityFrameworkCore.Migrations;

namespace FindProperty.Migrations
{
    public partial class change_column_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Agent",
                table: "Agent");

            migrationBuilder.DropColumn(
                name: "id",
                table: "Agent");

            migrationBuilder.AlterColumn<int>(
                name: "agent_id",
                table: "Property",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "AgentId",
                table: "Property",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AgentId",
                table: "Agent",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Agent",
                table: "Agent",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Property_AgentId",
                table: "Property",
                column: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Property_Agent_AgentId",
                table: "Property",
                column: "AgentId",
                principalTable: "Agent",
                principalColumn: "AgentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Property_Agent_AgentId",
                table: "Property");

            migrationBuilder.DropIndex(
                name: "IX_Property_AgentId",
                table: "Property");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Agent",
                table: "Agent");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Property");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Agent");

            migrationBuilder.AlterColumn<string>(
                name: "agent_id",
                table: "Property",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "Agent",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Agent",
                table: "Agent",
                column: "id");
        }
    }
}
