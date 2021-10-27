using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class AddedconstraintstoIot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IoTs_Position_MapPositionId",
                table: "IoTs");

            migrationBuilder.AddForeignKey(
                name: "FK_IoTs_Position_MapPositionId",
                table: "IoTs",
                column: "MapPositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IoTs_Position_MapPositionId",
                table: "IoTs");

            migrationBuilder.AddForeignKey(
                name: "FK_IoTs_Position_MapPositionId",
                table: "IoTs",
                column: "MapPositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
