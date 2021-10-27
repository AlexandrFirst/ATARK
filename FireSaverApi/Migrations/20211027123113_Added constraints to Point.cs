using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class AddedconstraintstoPoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Point_Position_MapPositionId",
                table: "Point");

            migrationBuilder.AddForeignKey(
                name: "FK_Point_Position_MapPositionId",
                table: "Point",
                column: "MapPositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Point_Position_MapPositionId",
                table: "Point");

            migrationBuilder.AddForeignKey(
                name: "FK_Point_Position_MapPositionId",
                table: "Point",
                column: "MapPositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
