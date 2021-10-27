using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class AddedconstraintstoEvacplanentity2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvacuationPlans_ScaleModels_ScaleModelId",
                table: "EvacuationPlans");

            migrationBuilder.AddForeignKey(
                name: "FK_EvacuationPlans_ScaleModels_ScaleModelId",
                table: "EvacuationPlans",
                column: "ScaleModelId",
                principalTable: "ScaleModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvacuationPlans_ScaleModels_ScaleModelId",
                table: "EvacuationPlans");

            migrationBuilder.AddForeignKey(
                name: "FK_EvacuationPlans_ScaleModels_ScaleModelId",
                table: "EvacuationPlans",
                column: "ScaleModelId",
                principalTable: "ScaleModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
