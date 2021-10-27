using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class AddedconstraintstoCompartmententity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Compartment_CurrentCompartmentId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Compartment_CurrentCompartmentId",
                table: "Users",
                column: "CurrentCompartmentId",
                principalTable: "Compartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Compartment_CurrentCompartmentId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Compartment_CurrentCompartmentId",
                table: "Users",
                column: "CurrentCompartmentId",
                principalTable: "Compartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
