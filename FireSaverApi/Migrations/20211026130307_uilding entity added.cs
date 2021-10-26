using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class uildingentityadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResponsibleForBuildingId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuildingCenterPositionId = table.Column<int>(type: "int", nullable: true),
                    SafetyDistance = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buildings_Positions_BuildingCenterPositionId",
                        column: x => x.BuildingCenterPositionId,
                        principalTable: "Positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ResponsibleForBuildingId",
                table: "Users",
                column: "ResponsibleForBuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_BuildingCenterPositionId",
                table: "Buildings",
                column: "BuildingCenterPositionId",
                unique: true,
                filter: "[BuildingCenterPositionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Buildings_ResponsibleForBuildingId",
                table: "Users",
                column: "ResponsibleForBuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Buildings_ResponsibleForBuildingId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Users_ResponsibleForBuildingId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResponsibleForBuildingId",
                table: "Users");
        }
    }
}
