using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class ShelterEntityadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoutePoints");

            migrationBuilder.RenameColumn(
                name: "FromPixelYToCoordYCoef",
                table: "ScaleModels",
                newName: "ImageYToRealYProjectCoef");

            migrationBuilder.RenameColumn(
                name: "FromPixelXToCoordXCoef",
                table: "ScaleModels",
                newName: "ImageYToRealXProjectCoef");

            migrationBuilder.RenameColumn(
                name: "FromCoordYToPixelYCoef",
                table: "ScaleModels",
                newName: "ImageXToRealYProjectCoef");

            migrationBuilder.RenameColumn(
                name: "FromCoordXToPixelXCoef",
                table: "ScaleModels",
                newName: "ImageXToRealXProjectCoef");

            migrationBuilder.AddColumn<int>(
                name: "ShelterId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompartmentPointsDataPublicId",
                table: "Compartment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Buildings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Shelters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuildingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shelters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shelters_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ShelterId",
                table: "Users",
                column: "ShelterId");

            migrationBuilder.CreateIndex(
                name: "IX_Shelters_BuildingId",
                table: "Shelters",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Shelters_ShelterId",
                table: "Users",
                column: "ShelterId",
                principalTable: "Shelters",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Shelters_ShelterId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Shelters");

            migrationBuilder.DropIndex(
                name: "IX_Users_ShelterId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ShelterId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CompartmentPointsDataPublicId",
                table: "Compartment");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Buildings");

            migrationBuilder.RenameColumn(
                name: "ImageYToRealYProjectCoef",
                table: "ScaleModels",
                newName: "FromPixelYToCoordYCoef");

            migrationBuilder.RenameColumn(
                name: "ImageYToRealXProjectCoef",
                table: "ScaleModels",
                newName: "FromPixelXToCoordXCoef");

            migrationBuilder.RenameColumn(
                name: "ImageXToRealYProjectCoef",
                table: "ScaleModels",
                newName: "FromCoordYToPixelYCoef");

            migrationBuilder.RenameColumn(
                name: "ImageXToRealXProjectCoef",
                table: "ScaleModels",
                newName: "FromCoordXToPixelXCoef");

            migrationBuilder.CreateTable(
                name: "RoutePoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompartmentId = table.Column<int>(type: "int", nullable: true),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    MapPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentPointId = table.Column<int>(type: "int", nullable: true),
                    RoutePointType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutePoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoutePoints_Compartment_CompartmentId",
                        column: x => x.CompartmentId,
                        principalTable: "Compartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoutePoints_RoutePoints_ParentPointId",
                        column: x => x.ParentPointId,
                        principalTable: "RoutePoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoutePoints_CompartmentId",
                table: "RoutePoints",
                column: "CompartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutePoints_ParentPointId",
                table: "RoutePoints",
                column: "ParentPointId");
        }
    }
}
