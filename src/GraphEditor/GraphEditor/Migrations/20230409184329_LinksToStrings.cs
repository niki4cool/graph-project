using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphEditor.Migrations
{
    /// <inheritdoc />
    public partial class LinksToStrings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GraphLink_GraphNode_SourceId",
                table: "GraphLink");

            migrationBuilder.DropForeignKey(
                name: "FK_GraphLink_GraphNode_TargetId",
                table: "GraphLink");

            migrationBuilder.DropIndex(
                name: "IX_GraphLink_SourceId",
                table: "GraphLink");

            migrationBuilder.DropIndex(
                name: "IX_GraphLink_TargetId",
                table: "GraphLink");

            migrationBuilder.RenameColumn(
                name: "TargetId",
                table: "GraphLink",
                newName: "Target");

            migrationBuilder.RenameColumn(
                name: "SourceId",
                table: "GraphLink",
                newName: "Source");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Target",
                table: "GraphLink",
                newName: "TargetId");

            migrationBuilder.RenameColumn(
                name: "Source",
                table: "GraphLink",
                newName: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphLink_SourceId",
                table: "GraphLink",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphLink_TargetId",
                table: "GraphLink",
                column: "TargetId");

            migrationBuilder.AddForeignKey(
                name: "FK_GraphLink_GraphNode_SourceId",
                table: "GraphLink",
                column: "SourceId",
                principalTable: "GraphNode",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GraphLink_GraphNode_TargetId",
                table: "GraphLink",
                column: "TargetId",
                principalTable: "GraphNode",
                principalColumn: "Id");
        }
    }
}
