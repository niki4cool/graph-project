using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraphEditor.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GraphRecordUserRecord_GraphRecord_CanEditId",
                table: "GraphRecordUserRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_GraphRecordUserRecord_UserRecord_CanEditId1",
                table: "GraphRecordUserRecord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GraphRecordUserRecord",
                table: "GraphRecordUserRecord");

            migrationBuilder.RenameTable(
                name: "GraphRecordUserRecord",
                newName: "Editors");

            migrationBuilder.RenameColumn(
                name: "CanEditId1",
                table: "Editors",
                newName: "EditorsId");

            migrationBuilder.RenameIndex(
                name: "IX_GraphRecordUserRecord_CanEditId1",
                table: "Editors",
                newName: "IX_Editors_EditorsId");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "GraphRecord",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Editors",
                table: "Editors",
                columns: new[] { "CanEditId", "EditorsId" });

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
                name: "IX_GraphRecord_CreatorId",
                table: "GraphRecord",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Viewers_ViewersId",
                table: "Viewers",
                column: "ViewersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Editors_GraphRecord_CanEditId",
                table: "Editors",
                column: "CanEditId",
                principalTable: "GraphRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Editors_UserRecord_EditorsId",
                table: "Editors",
                column: "EditorsId",
                principalTable: "UserRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GraphRecord_UserRecord_CreatorId",
                table: "GraphRecord",
                column: "CreatorId",
                principalTable: "UserRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Editors_GraphRecord_CanEditId",
                table: "Editors");

            migrationBuilder.DropForeignKey(
                name: "FK_Editors_UserRecord_EditorsId",
                table: "Editors");

            migrationBuilder.DropForeignKey(
                name: "FK_GraphRecord_UserRecord_CreatorId",
                table: "GraphRecord");

            migrationBuilder.DropTable(
                name: "Viewers");

            migrationBuilder.DropIndex(
                name: "IX_GraphRecord_CreatorId",
                table: "GraphRecord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Editors",
                table: "Editors");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "GraphRecord");

            migrationBuilder.RenameTable(
                name: "Editors",
                newName: "GraphRecordUserRecord");

            migrationBuilder.RenameColumn(
                name: "EditorsId",
                table: "GraphRecordUserRecord",
                newName: "CanEditId1");

            migrationBuilder.RenameIndex(
                name: "IX_Editors_EditorsId",
                table: "GraphRecordUserRecord",
                newName: "IX_GraphRecordUserRecord_CanEditId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GraphRecordUserRecord",
                table: "GraphRecordUserRecord",
                columns: new[] { "CanEditId", "CanEditId1" });

            migrationBuilder.AddForeignKey(
                name: "FK_GraphRecordUserRecord_GraphRecord_CanEditId",
                table: "GraphRecordUserRecord",
                column: "CanEditId",
                principalTable: "GraphRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GraphRecordUserRecord_UserRecord_CanEditId1",
                table: "GraphRecordUserRecord",
                column: "CanEditId1",
                principalTable: "UserRecord",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
