using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class userpostiononetooneparentchangeddelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Position_Users_UserId",
                table: "Position");

            migrationBuilder.AddForeignKey(
                name: "FK_Position_Users_UserId",
                table: "Position",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Position_Users_UserId",
                table: "Position");

            migrationBuilder.AddForeignKey(
                name: "FK_Position_Users_UserId",
                table: "Position",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
