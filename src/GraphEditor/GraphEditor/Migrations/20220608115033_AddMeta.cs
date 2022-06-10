using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphEditor.Migrations
{
    public partial class AddMeta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Meta_Color",
                table: "GraphNode",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Meta_Type",
                table: "GraphNode",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Meta_Color",
                table: "GraphNode");

            migrationBuilder.DropColumn(
                name: "Meta_Type",
                table: "GraphNode");
        }
    }
}
