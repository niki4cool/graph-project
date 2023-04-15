using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphEditor.Migrations
{
    /// <inheritdoc />
    public partial class NewGraphs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GraphLink_GraphRecord_GraphRecordId",
                table: "GraphLink");

            migrationBuilder.DropForeignKey(
                name: "FK_GraphNode_GraphRecord_GraphRecordId",
                table: "GraphNode");

            migrationBuilder.RenameColumn(
                name: "GraphRecordId",
                table: "GraphNode",
                newName: "GraphDataId");

            migrationBuilder.RenameIndex(
                name: "IX_GraphNode_GraphRecordId",
                table: "GraphNode",
                newName: "IX_GraphNode_GraphDataId");

            migrationBuilder.RenameColumn(
                name: "GraphRecordId",
                table: "GraphLink",
                newName: "GraphDataId");

            migrationBuilder.RenameIndex(
                name: "IX_GraphLink_GraphRecordId",
                table: "GraphLink",
                newName: "IX_GraphLink_GraphDataId");

            migrationBuilder.AddColumn<string>(
                name: "DataId",
                table: "GraphRecord",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GraphData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GraphRecord_DataId",
                table: "GraphRecord",
                column: "DataId");

            migrationBuilder.AddForeignKey(
                name: "FK_GraphLink_GraphData_GraphDataId",
                table: "GraphLink",
                column: "GraphDataId",
                principalTable: "GraphData",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GraphNode_GraphData_GraphDataId",
                table: "GraphNode",
                column: "GraphDataId",
                principalTable: "GraphData",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GraphRecord_GraphData_DataId",
                table: "GraphRecord",
                column: "DataId",
                principalTable: "GraphData",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GraphLink_GraphData_GraphDataId",
                table: "GraphLink");

            migrationBuilder.DropForeignKey(
                name: "FK_GraphNode_GraphData_GraphDataId",
                table: "GraphNode");

            migrationBuilder.DropForeignKey(
                name: "FK_GraphRecord_GraphData_DataId",
                table: "GraphRecord");

            migrationBuilder.DropTable(
                name: "GraphData");

            migrationBuilder.DropIndex(
                name: "IX_GraphRecord_DataId",
                table: "GraphRecord");

            migrationBuilder.DropColumn(
                name: "DataId",
                table: "GraphRecord");

            migrationBuilder.RenameColumn(
                name: "GraphDataId",
                table: "GraphNode",
                newName: "GraphRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_GraphNode_GraphDataId",
                table: "GraphNode",
                newName: "IX_GraphNode_GraphRecordId");

            migrationBuilder.RenameColumn(
                name: "GraphDataId",
                table: "GraphLink",
                newName: "GraphRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_GraphLink_GraphDataId",
                table: "GraphLink",
                newName: "IX_GraphLink_GraphRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_GraphLink_GraphRecord_GraphRecordId",
                table: "GraphLink",
                column: "GraphRecordId",
                principalTable: "GraphRecord",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GraphNode_GraphRecord_GraphRecordId",
                table: "GraphNode",
                column: "GraphRecordId",
                principalTable: "GraphRecord",
                principalColumn: "Id");
        }
    }
}
