using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GraphEditor.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GraphRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GraphLink",
                columns: table => new
                {
                    GraphDataGraphRecordId = table.Column<string>(type: "text", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Source = table.Column<string>(type: "text", nullable: false),
                    Target = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphLink", x => new { x.GraphDataGraphRecordId, x.Id });
                    table.ForeignKey(
                        name: "FK_GraphLink_GraphRecords_GraphDataGraphRecordId",
                        column: x => x.GraphDataGraphRecordId,
                        principalTable: "GraphRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GraphNode",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    GraphDataGraphRecordId = table.Column<string>(type: "text", nullable: false),
                    X = table.Column<float>(type: "real", nullable: false),
                    Y = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphNode", x => new { x.GraphDataGraphRecordId, x.Id });
                    table.ForeignKey(
                        name: "FK_GraphNode_GraphRecords_GraphDataGraphRecordId",
                        column: x => x.GraphDataGraphRecordId,
                        principalTable: "GraphRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GraphLink");

            migrationBuilder.DropTable(
                name: "GraphNode");

            migrationBuilder.DropTable(
                name: "GraphRecords");
        }
    }
}
