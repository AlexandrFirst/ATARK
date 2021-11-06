using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class scalepointposdeletestartegychanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EvacuationPlans_ScaleModels_ScaleModelId",
                table: "EvacuationPlans");

            migrationBuilder.DropIndex(
                name: "IX_EvacuationPlans_ScaleModelId",
                table: "EvacuationPlans");

            migrationBuilder.DropColumn(
                name: "ScaleModelId",
                table: "EvacuationPlans");

            migrationBuilder.AddColumn<int>(
                name: "ApplyingEvacPlansId",
                table: "ScaleModels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScaleModels_ApplyingEvacPlansId",
                table: "ScaleModels",
                column: "ApplyingEvacPlansId",
                unique: true,
                filter: "[ApplyingEvacPlansId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ScaleModels_EvacuationPlans_ApplyingEvacPlansId",
                table: "ScaleModels",
                column: "ApplyingEvacPlansId",
                principalTable: "EvacuationPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScaleModels_EvacuationPlans_ApplyingEvacPlansId",
                table: "ScaleModels");

            migrationBuilder.DropIndex(
                name: "IX_ScaleModels_ApplyingEvacPlansId",
                table: "ScaleModels");

            migrationBuilder.DropColumn(
                name: "ApplyingEvacPlansId",
                table: "ScaleModels");

            migrationBuilder.AddColumn<int>(
                name: "ScaleModelId",
                table: "EvacuationPlans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvacuationPlans_ScaleModelId",
                table: "EvacuationPlans",
                column: "ScaleModelId",
                unique: true,
                filter: "[ScaleModelId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_EvacuationPlans_ScaleModels_ScaleModelId",
                table: "EvacuationPlans",
                column: "ScaleModelId",
                principalTable: "ScaleModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
