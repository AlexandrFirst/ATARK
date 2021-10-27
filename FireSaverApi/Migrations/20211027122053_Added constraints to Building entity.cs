using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class AddedconstraintstoBuildingentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Position_BuildingCenterPositionId",
                table: "Buildings");

            migrationBuilder.DropForeignKey(
                name: "FK_Compartment_Buildings_BuildingWithThisFloorId",
                table: "Compartment");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Buildings_ResponsibleForBuildingId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Position_BuildingCenterPositionId",
                table: "Buildings",
                column: "BuildingCenterPositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Compartment_Buildings_BuildingWithThisFloorId",
                table: "Compartment",
                column: "BuildingWithThisFloorId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Buildings_ResponsibleForBuildingId",
                table: "Users",
                column: "ResponsibleForBuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Position_BuildingCenterPositionId",
                table: "Buildings");

            migrationBuilder.DropForeignKey(
                name: "FK_Compartment_Buildings_BuildingWithThisFloorId",
                table: "Compartment");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Buildings_ResponsibleForBuildingId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Position_BuildingCenterPositionId",
                table: "Buildings",
                column: "BuildingCenterPositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Compartment_Buildings_BuildingWithThisFloorId",
                table: "Compartment",
                column: "BuildingWithThisFloorId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Buildings_ResponsibleForBuildingId",
                table: "Users",
                column: "ResponsibleForBuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
