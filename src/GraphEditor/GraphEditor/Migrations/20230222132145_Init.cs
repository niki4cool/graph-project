using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphEditor.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRecord",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    NormalizedName = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRecord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GraphRecord",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserRecordId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraphRecord_UserRecord_UserRecordId",
                        column: x => x.UserRecordId,
                        principalTable: "UserRecord",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GraphNode",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    X = table.Column<float>(type: "real", nullable: false),
                    Y = table.Column<float>(type: "real", nullable: false),
                    Meta_Color = table.Column<string>(type: "text", nullable: true),
                    Meta_Type = table.Column<string>(type: "text", nullable: true),
                    GraphRecordId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphNode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraphNode_GraphRecord_GraphRecordId",
                        column: x => x.GraphRecordId,
                        principalTable: "GraphRecord",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GraphLink",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    SourceId = table.Column<string>(type: "text", nullable: true),
                    TargetId = table.Column<string>(type: "text", nullable: true),
                    GraphRecordId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraphLink_GraphNode_SourceId",
                        column: x => x.SourceId,
                        principalTable: "GraphNode",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GraphLink_GraphNode_TargetId",
                        column: x => x.TargetId,
                        principalTable: "GraphNode",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GraphLink_GraphRecord_GraphRecordId",
                        column: x => x.GraphRecordId,
                        principalTable: "GraphRecord",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GraphLink_GraphRecordId",
                table: "GraphLink",
                column: "GraphRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphLink_SourceId",
                table: "GraphLink",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphLink_TargetId",
                table: "GraphLink",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphNode_GraphRecordId",
                table: "GraphNode",
                column: "GraphRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphRecord_UserRecordId",
                table: "GraphRecord",
                column: "UserRecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GraphLink");

            migrationBuilder.DropTable(
                name: "GraphNode");

            migrationBuilder.DropTable(
                name: "GraphRecord");

            migrationBuilder.DropTable(
                name: "UserRecord");
        }
    }
}
