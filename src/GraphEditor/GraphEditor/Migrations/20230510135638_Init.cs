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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NormalizedName = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Node",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Meta_X = table.Column<float>(type: "real", nullable: false),
                    Meta_Y = table.Column<float>(type: "real", nullable: false),
                    Meta_Name = table.Column<string>(type: "text", nullable: false),
                    Meta_NodeClass = table.Column<string>(type: "text", nullable: false),
                    Meta_Color = table.Column<string>(type: "text", nullable: false),
                    GraphDataId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Node", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Node_GraphData_GraphDataId",
                        column: x => x.GraphDataId,
                        principalTable: "GraphData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Graphs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    DataId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    GraphType = table.Column<int>(type: "integer", nullable: false),
                    ClassGraphId = table.Column<string>(type: "text", nullable: true),
                    CreatorId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Graphs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Graphs_GraphData_DataId",
                        column: x => x.DataId,
                        principalTable: "GraphData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Graphs_Graphs_ClassGraphId",
                        column: x => x.ClassGraphId,
                        principalTable: "Graphs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Graphs_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Edge",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FromId = table.Column<string>(type: "text", nullable: false),
                    ToId = table.Column<string>(type: "text", nullable: false),
                    Meta_Value = table.Column<float>(type: "real", nullable: false),
                    GraphDataId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Edge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Edge_GraphData_GraphDataId",
                        column: x => x.GraphDataId,
                        principalTable: "GraphData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Edge_Node_FromId",
                        column: x => x.FromId,
                        principalTable: "Node",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Edge_Node_ToId",
                        column: x => x.ToId,
                        principalTable: "Node",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GraphUser_Edit",
                columns: table => new
                {
                    CanBeEditedById = table.Column<string>(type: "text", nullable: false),
                    CanEditId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphUser_Edit", x => new { x.CanBeEditedById, x.CanEditId });
                    table.ForeignKey(
                        name: "FK_GraphUser_Edit_Graphs_CanEditId",
                        column: x => x.CanEditId,
                        principalTable: "Graphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GraphUser_Edit_Users_CanBeEditedById",
                        column: x => x.CanBeEditedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GraphUser_View",
                columns: table => new
                {
                    CanBeViewedById = table.Column<string>(type: "text", nullable: false),
                    CanViewId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphUser_View", x => new { x.CanBeViewedById, x.CanViewId });
                    table.ForeignKey(
                        name: "FK_GraphUser_View_Graphs_CanViewId",
                        column: x => x.CanViewId,
                        principalTable: "Graphs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GraphUser_View_Users_CanBeViewedById",
                        column: x => x.CanBeViewedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Edge_FromId",
                table: "Edge",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Edge_GraphDataId",
                table: "Edge",
                column: "GraphDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Edge_ToId",
                table: "Edge",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_Graphs_ClassGraphId",
                table: "Graphs",
                column: "ClassGraphId");

            migrationBuilder.CreateIndex(
                name: "IX_Graphs_CreatorId",
                table: "Graphs",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Graphs_DataId",
                table: "Graphs",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphUser_Edit_CanEditId",
                table: "GraphUser_Edit",
                column: "CanEditId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphUser_View_CanViewId",
                table: "GraphUser_View",
                column: "CanViewId");

            migrationBuilder.CreateIndex(
                name: "IX_Node_GraphDataId",
                table: "Node",
                column: "GraphDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Edge");

            migrationBuilder.DropTable(
                name: "GraphUser_Edit");

            migrationBuilder.DropTable(
                name: "GraphUser_View");

            migrationBuilder.DropTable(
                name: "Node");

            migrationBuilder.DropTable(
                name: "Graphs");

            migrationBuilder.DropTable(
                name: "GraphData");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
