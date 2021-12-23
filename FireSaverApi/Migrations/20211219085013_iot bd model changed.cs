using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class iotbdmodelchanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastRecordedAmmoniaLevel",
                table: "IoTs");

            migrationBuilder.DropColumn(
                name: "LastRecordedCO2Level",
                table: "IoTs");

            migrationBuilder.DropColumn(
                name: "LastRecordedNitrogenOxidesLevel",
                table: "IoTs");

            migrationBuilder.DropColumn(
                name: "LastRecordedPetrolLevel",
                table: "IoTs");

            migrationBuilder.DropColumn(
                name: "LastRecordedSmokeLevel",
                table: "IoTs");

            migrationBuilder.DropColumn(
                name: "LastRecordedTemperature",
                table: "IoTs");

            migrationBuilder.AddColumn<float>(
                name: "SensorValue",
                table: "IoTs",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SensorValue",
                table: "IoTs");

            migrationBuilder.AddColumn<double>(
                name: "LastRecordedAmmoniaLevel",
                table: "IoTs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LastRecordedCO2Level",
                table: "IoTs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LastRecordedNitrogenOxidesLevel",
                table: "IoTs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LastRecordedPetrolLevel",
                table: "IoTs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LastRecordedSmokeLevel",
                table: "IoTs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LastRecordedTemperature",
                table: "IoTs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
