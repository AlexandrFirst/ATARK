using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class iotmessagesadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IoTId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_IoTId",
                table: "Messages",
                column: "IoTId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_IoTs_IoTId",
                table: "Messages",
                column: "IoTId",
                principalTable: "IoTs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_IoTs_IoTId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_IoTId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IoTId",
                table: "Messages");
        }
    }
}
