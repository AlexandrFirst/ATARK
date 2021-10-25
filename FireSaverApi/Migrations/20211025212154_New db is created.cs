using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class Newdbiscreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SafetyDistance = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EvacuationPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvacuationPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TryCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScaleModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CoordToPixelCoef = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScaleModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScaleModels_EvacuationPlans_Id",
                        column: x => x.Id,
                        principalTable: "EvacuationPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Compartment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SafetyRules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: true),
                    CurrentFloorId = table.Column<int>(type: "int", nullable: true),
                    BuildingWithThisFloorId = table.Column<int>(type: "int", nullable: true),
                    RoomFloorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compartment_Buildings_BuildingWithThisFloorId",
                        column: x => x.BuildingWithThisFloorId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compartment_Compartment_CurrentFloorId",
                        column: x => x.CurrentFloorId,
                        principalTable: "Compartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compartment_Compartment_RoomFloorId",
                        column: x => x.RoomFloorId,
                        principalTable: "Compartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compartment_EvacuationPlans_Id",
                        column: x => x.Id,
                        principalTable: "EvacuationPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Compartment_Tests_Id",
                        column: x => x.Id,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<int>(type: "int", nullable: false),
                    AnswearsList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IoTs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAuthNeeded = table.Column<bool>(type: "bit", nullable: false),
                    LastRecordedTemperature = table.Column<double>(type: "float", nullable: false),
                    LastRecordedCO2Level = table.Column<double>(type: "float", nullable: false),
                    LastRecordedAmmoniaLevel = table.Column<double>(type: "float", nullable: false),
                    LastRecordedNitrogenOxidesLevel = table.Column<double>(type: "float", nullable: false),
                    LastRecordedSmokeLevel = table.Column<double>(type: "float", nullable: false),
                    LastRecordedPetrolLevel = table.Column<double>(type: "float", nullable: false),
                    CompartmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IoTs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IoTs_Compartment_CompartmentId",
                        column: x => x.CompartmentId,
                        principalTable: "Compartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Point",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentPointId = table.Column<int>(type: "int", nullable: true),
                    RoutePointType = table.Column<int>(type: "int", nullable: true),
                    CompartmentId = table.Column<int>(type: "int", nullable: true),
                    ScaleModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Point", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Point_Compartment_CompartmentId",
                        column: x => x.CompartmentId,
                        principalTable: "Compartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Point_Point_ParentPointId",
                        column: x => x.ParentPointId,
                        principalTable: "Point",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Point_ScaleModels_ScaleModelId",
                        column: x => x.ScaleModelId,
                        principalTable: "ScaleModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolesList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Patronymic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelephoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponsibleForBuildingId = table.Column<int>(type: "int", nullable: true),
                    CurrentCompartmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Buildings_ResponsibleForBuildingId",
                        column: x => x.ResponsibleForBuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Compartment_CurrentCompartmentId",
                        column: x => x.CurrentCompartmentId,
                        principalTable: "Compartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Longtitude = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Position_Buildings_Id",
                        column: x => x.Id,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Position_IoTs_Id",
                        column: x => x.Id,
                        principalTable: "IoTs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Position_Point_Id",
                        column: x => x.Id,
                        principalTable: "Point",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Position_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compartment_BuildingWithThisFloorId",
                table: "Compartment",
                column: "BuildingWithThisFloorId");

            migrationBuilder.CreateIndex(
                name: "IX_Compartment_CurrentFloorId",
                table: "Compartment",
                column: "CurrentFloorId");

            migrationBuilder.CreateIndex(
                name: "IX_Compartment_RoomFloorId",
                table: "Compartment",
                column: "RoomFloorId");

            migrationBuilder.CreateIndex(
                name: "IX_IoTs_CompartmentId",
                table: "IoTs",
                column: "CompartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Point_CompartmentId",
                table: "Point",
                column: "CompartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Point_ParentPointId",
                table: "Point",
                column: "ParentPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Point_ScaleModelId",
                table: "Point",
                column: "ScaleModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TestId",
                table: "Questions",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentCompartmentId",
                table: "Users",
                column: "CurrentCompartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ResponsibleForBuildingId",
                table: "Users",
                column: "ResponsibleForBuildingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "IoTs");

            migrationBuilder.DropTable(
                name: "Point");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ScaleModels");

            migrationBuilder.DropTable(
                name: "Compartment");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "EvacuationPlans");

            migrationBuilder.DropTable(
                name: "Tests");
        }
    }
}
