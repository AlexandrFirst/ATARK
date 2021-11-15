using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FireSaverApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuildingCenterPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromPixelXToCoordXCoef = table.Column<double>(type: "float", nullable: false),
                    FromCoordXToPixelXCoef = table.Column<double>(type: "float", nullable: false),
                    FromPixelYToCoordYCoef = table.Column<double>(type: "float", nullable: false),
                    FromCoordYToPixelYCoef = table.Column<double>(type: "float", nullable: false),
                    ApplyingEvacPlansId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScaleModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScaleModels_EvacuationPlans_ApplyingEvacPlansId",
                        column: x => x.ApplyingEvacPlansId,
                        principalTable: "EvacuationPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Compartment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvacuationPlanId = table.Column<int>(type: "int", nullable: true),
                    SafetyRules = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompartmentTestId = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: true),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Compartment_Compartment_RoomFloorId",
                        column: x => x.RoomFloorId,
                        principalTable: "Compartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compartment_EvacuationPlans_EvacuationPlanId",
                        column: x => x.EvacuationPlanId,
                        principalTable: "EvacuationPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Compartment_Tests_CompartmentTestId",
                        column: x => x.CompartmentTestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswearsList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PossibleAnswears = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "ScalePoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorldPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScaleModelId = table.Column<int>(type: "int", nullable: true),
                    MapPosition = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScalePoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScalePoints_ScaleModels_ScaleModelId",
                        column: x => x.ScaleModelId,
                        principalTable: "ScaleModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IoTs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MapPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IotIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "RoutePoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentPointId = table.Column<int>(type: "int", nullable: true),
                    RoutePointType = table.Column<int>(type: "int", nullable: false),
                    CompartmentId = table.Column<int>(type: "int", nullable: true),
                    MapPosition = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mail = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TelephoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastSeenBuildingPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Compartment_CurrentCompartmentId",
                        column: x => x.CurrentCompartmentId,
                        principalTable: "Compartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compartment_BuildingWithThisFloorId",
                table: "Compartment",
                column: "BuildingWithThisFloorId");

            migrationBuilder.CreateIndex(
                name: "IX_Compartment_CompartmentTestId",
                table: "Compartment",
                column: "CompartmentTestId",
                unique: true,
                filter: "[CompartmentTestId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Compartment_EvacuationPlanId",
                table: "Compartment",
                column: "EvacuationPlanId",
                unique: true,
                filter: "[EvacuationPlanId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Compartment_RoomFloorId",
                table: "Compartment",
                column: "RoomFloorId");

            migrationBuilder.CreateIndex(
                name: "IX_IoTs_CompartmentId",
                table: "IoTs",
                column: "CompartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TestId",
                table: "Questions",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutePoints_CompartmentId",
                table: "RoutePoints",
                column: "CompartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutePoints_ParentPointId",
                table: "RoutePoints",
                column: "ParentPointId");

            migrationBuilder.CreateIndex(
                name: "IX_ScaleModels_ApplyingEvacPlansId",
                table: "ScaleModels",
                column: "ApplyingEvacPlansId",
                unique: true,
                filter: "[ApplyingEvacPlansId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ScalePoints_ScaleModelId",
                table: "ScalePoints",
                column: "ScaleModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrentCompartmentId",
                table: "Users",
                column: "CurrentCompartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Mail",
                table: "Users",
                column: "Mail",
                unique: true,
                filter: "[Mail] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ResponsibleForBuildingId",
                table: "Users",
                column: "ResponsibleForBuildingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IoTs");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "RoutePoints");

            migrationBuilder.DropTable(
                name: "ScalePoints");

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
