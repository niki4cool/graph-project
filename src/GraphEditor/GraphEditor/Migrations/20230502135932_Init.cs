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
                name: "GraphData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphData", x => x.Id);
                });

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
                name: "GraphLink",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: true),
                    Target = table.Column<string>(type: "text", nullable: true),
                    GraphDataId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraphLink_GraphData_GraphDataId",
                        column: x => x.GraphDataId,
                        principalTable: "GraphData",
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
                    GraphDataId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphNode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraphNode_GraphData_GraphDataId",
                        column: x => x.GraphDataId,
                        principalTable: "GraphData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GraphRecord",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    DataId = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraphRecord_GraphData_DataId",
                        column: x => x.DataId,
                        principalTable: "GraphData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GraphRecord_UserRecord_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "UserRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Editors",
                columns: table => new
                {
                    CanEditId = table.Column<string>(type: "text", nullable: false),
                    EditorsId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Editors", x => new { x.CanEditId, x.EditorsId });
                    table.ForeignKey(
                        name: "FK_Editors_GraphRecord_CanEditId",
                        column: x => x.CanEditId,
                        principalTable: "GraphRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Editors_UserRecord_EditorsId",
                        column: x => x.EditorsId,
                        principalTable: "UserRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Viewers",
                columns: table => new
                {
                    CanViewId = table.Column<string>(type: "text", nullable: false),
                    ViewersId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Viewers", x => new { x.CanViewId, x.ViewersId });
                    table.ForeignKey(
                        name: "FK_Viewers_GraphRecord_CanViewId",
                        column: x => x.CanViewId,
                        principalTable: "GraphRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Viewers_UserRecord_ViewersId",
                        column: x => x.ViewersId,
                        principalTable: "UserRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Editors_EditorsId",
                table: "Editors",
                column: "EditorsId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphLink_GraphDataId",
                table: "GraphLink",
                column: "GraphDataId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphNode_GraphDataId",
                table: "GraphNode",
                column: "GraphDataId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphRecord_CreatorId",
                table: "GraphRecord",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphRecord_DataId",
                table: "GraphRecord",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_Viewers_ViewersId",
                table: "Viewers",
                column: "ViewersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Editors");

            migrationBuilder.DropTable(
                name: "GraphLink");

            migrationBuilder.DropTable(
                name: "GraphNode");

            migrationBuilder.DropTable(
                name: "Viewers");

            migrationBuilder.DropTable(
                name: "GraphRecord");

            migrationBuilder.DropTable(
                name: "GraphData");

            migrationBuilder.DropTable(
                name: "UserRecord");
        }
    }
}
