using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class Iotbuildingrefadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuildingId",
                table: "IoTs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IotIdentifier",
                table: "IoTs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IoTs_BuildingId",
                table: "IoTs",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_IoTs_Buildings_BuildingId",
                table: "IoTs",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IoTs_Buildings_BuildingId",
                table: "IoTs");

            migrationBuilder.DropIndex(
                name: "IX_IoTs_BuildingId",
                table: "IoTs");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "IoTs");

            migrationBuilder.DropColumn(
                name: "IotIdentifier",
                table: "IoTs");
        }
    }
}
