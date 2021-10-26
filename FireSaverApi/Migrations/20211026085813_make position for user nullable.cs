using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class makepositionforusernullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_LastSeenBuildingPositionId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "LastSeenBuildingPositionId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastSeenBuildingPositionId",
                table: "Users",
                column: "LastSeenBuildingPositionId",
                unique: true,
                filter: "[LastSeenBuildingPositionId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_LastSeenBuildingPositionId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "LastSeenBuildingPositionId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastSeenBuildingPositionId",
                table: "Users",
                column: "LastSeenBuildingPositionId",
                unique: true);
        }
    }
}
