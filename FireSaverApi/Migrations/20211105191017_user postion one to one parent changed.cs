using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class userpostiononetooneparentchanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Position_LastSeenBuildingPositionId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_LastSeenBuildingPositionId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastSeenBuildingPositionId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Position",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "EvacuationPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "EvacuationPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Position_UserId",
                table: "Position",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Position_Users_UserId",
                table: "Position",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Position_Users_UserId",
                table: "Position");

            migrationBuilder.DropIndex(
                name: "IX_Position_UserId",
                table: "Position");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Position");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "EvacuationPlans");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "EvacuationPlans");

            migrationBuilder.AddColumn<int>(
                name: "LastSeenBuildingPositionId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastSeenBuildingPositionId",
                table: "Users",
                column: "LastSeenBuildingPositionId",
                unique: true,
                filter: "[LastSeenBuildingPositionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Position_LastSeenBuildingPositionId",
                table: "Users",
                column: "LastSeenBuildingPositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
