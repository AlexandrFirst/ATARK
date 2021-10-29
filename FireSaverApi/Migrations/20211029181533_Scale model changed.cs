using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class Scalemodelchanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoordToPixelCoef",
                table: "ScaleModels",
                newName: "FromPixelYToCoordYCoef");

            migrationBuilder.AddColumn<double>(
                name: "FromCoordXToPixelXCoef",
                table: "ScaleModels",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FromCoordYToPixelYCoef",
                table: "ScaleModels",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FromPixelXToCoordXCoef",
                table: "ScaleModels",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromCoordXToPixelXCoef",
                table: "ScaleModels");

            migrationBuilder.DropColumn(
                name: "FromCoordYToPixelYCoef",
                table: "ScaleModels");

            migrationBuilder.DropColumn(
                name: "FromPixelXToCoordXCoef",
                table: "ScaleModels");

            migrationBuilder.RenameColumn(
                name: "FromPixelYToCoordYCoef",
                table: "ScaleModels",
                newName: "CoordToPixelCoef");
        }
    }
}
