using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class buildingpostiononetooneparentchanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Position_BuildingCenterPositionId",
                table: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_BuildingCenterPositionId",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "BuildingCenterPositionId",
                table: "Buildings");

            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "Position",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Position_BuildingId",
                table: "Position",
                column: "BuildingId",
                unique: true,
                filter: "[BuildingId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Position_Buildings_BuildingId",
                table: "Position",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Position_Buildings_BuildingId",
                table: "Position");

            migrationBuilder.DropIndex(
                name: "IX_Position_BuildingId",
                table: "Position");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Position");

            migrationBuilder.AddColumn<int>(
                name: "BuildingCenterPositionId",
                table: "Buildings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_BuildingCenterPositionId",
                table: "Buildings",
                column: "BuildingCenterPositionId",
                unique: true,
                filter: "[BuildingCenterPositionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Position_BuildingCenterPositionId",
                table: "Buildings",
                column: "BuildingCenterPositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
