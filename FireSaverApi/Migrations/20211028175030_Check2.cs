using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class Check2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compartment_Compartment_CurrentFloorId",
                table: "Compartment");

            migrationBuilder.DropIndex(
                name: "IX_Compartment_CurrentFloorId",
                table: "Compartment");

            migrationBuilder.DropColumn(
                name: "CurrentFloorId",
                table: "Compartment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentFloorId",
                table: "Compartment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Compartment_CurrentFloorId",
                table: "Compartment",
                column: "CurrentFloorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Compartment_Compartment_CurrentFloorId",
                table: "Compartment",
                column: "CurrentFloorId",
                principalTable: "Compartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
