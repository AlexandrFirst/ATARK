using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class AddedconstraintstoUSer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Position_LastSeenBuildingPositionId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Position_LastSeenBuildingPositionId",
                table: "Users",
                column: "LastSeenBuildingPositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Position_LastSeenBuildingPositionId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Position_LastSeenBuildingPositionId",
                table: "Users",
                column: "LastSeenBuildingPositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
