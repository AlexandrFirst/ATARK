using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class iotpostiononetooneparentchangeddeletestrategy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IoTs_Position_MapPositionId",
                table: "IoTs");

            migrationBuilder.DropIndex(
                name: "IX_IoTs_MapPositionId",
                table: "IoTs");

            migrationBuilder.DropColumn(
                name: "MapPositionId",
                table: "IoTs");

            migrationBuilder.AddColumn<int>(
                name: "IotId",
                table: "Position",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Position_IotId",
                table: "Position",
                column: "IotId",
                unique: true,
                filter: "[IotId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Position_IoTs_IotId",
                table: "Position",
                column: "IotId",
                principalTable: "IoTs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Position_IoTs_IotId",
                table: "Position");

            migrationBuilder.DropIndex(
                name: "IX_Position_IotId",
                table: "Position");

            migrationBuilder.DropColumn(
                name: "IotId",
                table: "Position");

            migrationBuilder.AddColumn<int>(
                name: "MapPositionId",
                table: "IoTs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IoTs_MapPositionId",
                table: "IoTs",
                column: "MapPositionId",
                unique: true,
                filter: "[MapPositionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_IoTs_Position_MapPositionId",
                table: "IoTs",
                column: "MapPositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
