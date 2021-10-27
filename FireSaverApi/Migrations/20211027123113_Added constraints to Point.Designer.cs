﻿// <auto-generated />
using System;
using FireSaverApi.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FireSaverApi.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20211027123113_Added constraints to Point")]
    partial class AddedconstraintstoPoint
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FireSaverApi.DataContext.Building", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BuildingCenterPositionId")
                        .HasColumnType("int");

                    b.Property<double>("SafetyDistance")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("BuildingCenterPositionId")
                        .IsUnique()
                        .HasFilter("[BuildingCenterPositionId] IS NOT NULL");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Compartment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CompartmentTestId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EvacuationPlanId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SafetyRules")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompartmentTestId")
                        .IsUnique()
                        .HasFilter("[CompartmentTestId] IS NOT NULL");

                    b.HasIndex("EvacuationPlanId")
                        .IsUnique()
                        .HasFilter("[EvacuationPlanId] IS NOT NULL");

                    b.ToTable("Compartment");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Compartment");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.EvacuationPlan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PublicId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ScaleModelId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UploadTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ScaleModelId")
                        .IsUnique()
                        .HasFilter("[ScaleModelId] IS NOT NULL");

                    b.ToTable("EvacuationPlans");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.IoT", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CompartmentId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAuthNeeded")
                        .HasColumnType("bit");

                    b.Property<double>("LastRecordedAmmoniaLevel")
                        .HasColumnType("float");

                    b.Property<double>("LastRecordedCO2Level")
                        .HasColumnType("float");

                    b.Property<double>("LastRecordedNitrogenOxidesLevel")
                        .HasColumnType("float");

                    b.Property<double>("LastRecordedPetrolLevel")
                        .HasColumnType("float");

                    b.Property<double>("LastRecordedSmokeLevel")
                        .HasColumnType("float");

                    b.Property<double>("LastRecordedTemperature")
                        .HasColumnType("float");

                    b.Property<int?>("MapPositionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompartmentId");

                    b.HasIndex("MapPositionId")
                        .IsUnique()
                        .HasFilter("[MapPositionId] IS NOT NULL");

                    b.ToTable("IoTs");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Point", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MapPositionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MapPositionId")
                        .IsUnique()
                        .HasFilter("[MapPositionId] IS NOT NULL");

                    b.ToTable("Point");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Point");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longtitude")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Position");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AnswearsList")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Content")
                        .HasColumnType("int");

                    b.Property<int?>("TestId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.ScaleModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("CoordToPixelCoef")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("ScaleModels");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("TryCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CurrentCompartmentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<int?>("LastSeenBuildingPositionId")
                        .HasColumnType("int");

                    b.Property<string>("Mail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Patronymic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ResponsibleForBuildingId")
                        .HasColumnType("int");

                    b.Property<string>("RolesList")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelephoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CurrentCompartmentId");

                    b.HasIndex("LastSeenBuildingPositionId")
                        .IsUnique()
                        .HasFilter("[LastSeenBuildingPositionId] IS NOT NULL");

                    b.HasIndex("ResponsibleForBuildingId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Floor", b =>
                {
                    b.HasBaseType("FireSaverApi.DataContext.Compartment");

                    b.Property<int?>("BuildingWithThisFloorId")
                        .HasColumnType("int");

                    b.Property<int?>("CurrentFloorId")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.HasIndex("BuildingWithThisFloorId");

                    b.HasIndex("CurrentFloorId");

                    b.HasDiscriminator().HasValue("Floor");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Room", b =>
                {
                    b.HasBaseType("FireSaverApi.DataContext.Compartment");

                    b.Property<int?>("RoomFloorId")
                        .HasColumnType("int");

                    b.HasIndex("RoomFloorId");

                    b.HasDiscriminator().HasValue("Room");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.RoutePoint", b =>
                {
                    b.HasBaseType("FireSaverApi.DataContext.Point");

                    b.Property<int?>("CompartmentId")
                        .HasColumnType("int");

                    b.Property<int?>("ParentPointId")
                        .HasColumnType("int");

                    b.Property<int>("RoutePointType")
                        .HasColumnType("int");

                    b.HasIndex("CompartmentId");

                    b.HasIndex("ParentPointId");

                    b.HasDiscriminator().HasValue("RoutePoint");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.ScalePoint", b =>
                {
                    b.HasBaseType("FireSaverApi.DataContext.Point");

                    b.Property<int?>("ScaleModelId")
                        .HasColumnType("int");

                    b.Property<int?>("WorldPositionId")
                        .HasColumnType("int");

                    b.HasIndex("ScaleModelId");

                    b.HasIndex("WorldPositionId")
                        .IsUnique()
                        .HasFilter("[WorldPositionId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("ScalePoint");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Building", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.Position", "BuildingCenterPosition")
                        .WithOne("Building")
                        .HasForeignKey("FireSaverApi.DataContext.Building", "BuildingCenterPositionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("BuildingCenterPosition");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Compartment", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.Test", "CompartmentTest")
                        .WithOne("Compartment")
                        .HasForeignKey("FireSaverApi.DataContext.Compartment", "CompartmentTestId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("FireSaverApi.DataContext.EvacuationPlan", "EvacuationPlan")
                        .WithOne("Compartment")
                        .HasForeignKey("FireSaverApi.DataContext.Compartment", "EvacuationPlanId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("CompartmentTest");

                    b.Navigation("EvacuationPlan");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.EvacuationPlan", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.ScaleModel", "ScaleModel")
                        .WithOne("ApplyingEvacPlans")
                        .HasForeignKey("FireSaverApi.DataContext.EvacuationPlan", "ScaleModelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("ScaleModel");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.IoT", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.Compartment", "Compartment")
                        .WithMany("Iots")
                        .HasForeignKey("CompartmentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("FireSaverApi.DataContext.Position", "MapPosition")
                        .WithOne("IotPostion")
                        .HasForeignKey("FireSaverApi.DataContext.IoT", "MapPositionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Compartment");

                    b.Navigation("MapPosition");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Point", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.Position", "MapPosition")
                        .WithOne("PointPostion")
                        .HasForeignKey("FireSaverApi.DataContext.Point", "MapPositionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("MapPosition");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Question", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.Test", "Test")
                        .WithMany("Questions")
                        .HasForeignKey("TestId");

                    b.Navigation("Test");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.User", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.Compartment", "CurrentCompartment")
                        .WithMany("InboundUsers")
                        .HasForeignKey("CurrentCompartmentId");

                    b.HasOne("FireSaverApi.DataContext.Position", "LastSeenBuildingPosition")
                        .WithOne("User")
                        .HasForeignKey("FireSaverApi.DataContext.User", "LastSeenBuildingPositionId");

                    b.HasOne("FireSaverApi.DataContext.Building", "ResponsibleForBuilding")
                        .WithMany("ResponsibleUsers")
                        .HasForeignKey("ResponsibleForBuildingId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("CurrentCompartment");

                    b.Navigation("LastSeenBuildingPosition");

                    b.Navigation("ResponsibleForBuilding");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Floor", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.Building", "BuildingWithThisFloor")
                        .WithMany("Floors")
                        .HasForeignKey("BuildingWithThisFloorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FireSaverApi.DataContext.Floor", "CurrentFloor")
                        .WithMany("NearFloors")
                        .HasForeignKey("CurrentFloorId");

                    b.Navigation("BuildingWithThisFloor");

                    b.Navigation("CurrentFloor");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Room", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.Floor", "RoomFloor")
                        .WithMany("Rooms")
                        .HasForeignKey("RoomFloorId");

                    b.Navigation("RoomFloor");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.RoutePoint", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.Compartment", "Compartment")
                        .WithMany("RoutePoints")
                        .HasForeignKey("CompartmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FireSaverApi.DataContext.RoutePoint", "ParentPoint")
                        .WithMany("ChildrenPoints")
                        .HasForeignKey("ParentPointId");

                    b.Navigation("Compartment");

                    b.Navigation("ParentPoint");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.ScalePoint", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.ScaleModel", "ScaleModel")
                        .WithMany("ScalePoints")
                        .HasForeignKey("ScaleModelId");

                    b.HasOne("FireSaverApi.DataContext.Position", "WorldPosition")
                        .WithOne("ScalePoint")
                        .HasForeignKey("FireSaverApi.DataContext.ScalePoint", "WorldPositionId");

                    b.Navigation("ScaleModel");

                    b.Navigation("WorldPosition");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Building", b =>
                {
                    b.Navigation("Floors");

                    b.Navigation("ResponsibleUsers");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Compartment", b =>
                {
                    b.Navigation("InboundUsers");

                    b.Navigation("Iots");

                    b.Navigation("RoutePoints");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.EvacuationPlan", b =>
                {
                    b.Navigation("Compartment");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Position", b =>
                {
                    b.Navigation("Building");

                    b.Navigation("IotPostion");

                    b.Navigation("PointPostion");

                    b.Navigation("ScalePoint");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.ScaleModel", b =>
                {
                    b.Navigation("ApplyingEvacPlans");

                    b.Navigation("ScalePoints");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Test", b =>
                {
                    b.Navigation("Compartment");

                    b.Navigation("Questions");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Floor", b =>
                {
                    b.Navigation("NearFloors");

                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.RoutePoint", b =>
                {
                    b.Navigation("ChildrenPoints");
                });
#pragma warning restore 612, 618
        }
    }
}
