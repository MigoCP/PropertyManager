using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Property_Manager_WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerToBuildings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitNumber",
                table: "Apartments",
                newName: "Address");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Buildings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Reports_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_OwnerId",
                table: "Buildings",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ManagerId",
                table: "Reports",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_OwnerId",
                table: "Reports",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK__Buildings__Owner__4F7CD00D",
                table: "Buildings",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Buildings__Owner__4F7CD00D",
                table: "Buildings");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_OwnerId",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Buildings");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Apartments",
                newName: "UnitNumber");
        }
    }
}
