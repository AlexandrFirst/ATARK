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
    [Migration("20211107142009_User role entity updated")]
    partial class Userroleentityupdated
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

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BuildingCenterPosition")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Info")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("SafetyDistance")
                        .HasColumnType("float");

                    b.HasKey("Id");

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

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("PublicId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UploadTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

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

                    b.Property<string>("IotIdentifier")
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<string>("MapPosition")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompartmentId");

                    b.ToTable("IoTs");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AnswearsList")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PossibleAnswears")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TestId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.RoutePoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CompartmentId")
                        .HasColumnType("int");

                    b.Property<string>("MapPosition")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentPointId")
                        .HasColumnType("int");

                    b.Property<int>("RoutePointType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompartmentId");

                    b.HasIndex("ParentPointId");

                    b.ToTable("RoutePoints");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.ScaleModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ApplyingEvacPlansId")
                        .HasColumnType("int");

                    b.Property<double>("FromCoordXToPixelXCoef")
                        .HasColumnType("float");

                    b.Property<double>("FromCoordYToPixelYCoef")
                        .HasColumnType("float");

                    b.Property<double>("FromPixelXToCoordXCoef")
                        .HasColumnType("float");

                    b.Property<double>("FromPixelYToCoordYCoef")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ApplyingEvacPlansId")
                        .IsUnique()
                        .HasFilter("[ApplyingEvacPlansId] IS NOT NULL");

                    b.ToTable("ScaleModels");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.ScalePoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MapPosition")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ScaleModelId")
                        .HasColumnType("int");

                    b.Property<string>("WorldPosition")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ScaleModelId");

                    b.ToTable("ScalePoints");
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

                    b.Property<string>("LastSeenBuildingPosition")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mail")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Patronymic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ResponsibleForBuildingId")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelephoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CurrentCompartmentId");

                    b.HasIndex("Mail")
                        .IsUnique()
                        .HasFilter("[Mail] IS NOT NULL");

                    b.HasIndex("ResponsibleForBuildingId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("UserUserRole", b =>
                {
                    b.Property<int>("RolesListId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("RolesListId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("UserUserRole");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Floor", b =>
                {
                    b.HasBaseType("FireSaverApi.DataContext.Compartment");

                    b.Property<int?>("BuildingWithThisFloorId")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.HasIndex("BuildingWithThisFloorId");

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

            modelBuilder.Entity("FireSaverApi.DataContext.IoT", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.Compartment", "Compartment")
                        .WithMany("Iots")
                        .HasForeignKey("CompartmentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Compartment");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Question", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.Test", "Test")
                        .WithMany("Questions")
                        .HasForeignKey("TestId");

                    b.Navigation("Test");
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

            modelBuilder.Entity("FireSaverApi.DataContext.ScaleModel", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.EvacuationPlan", "ApplyingEvacPlans")
                        .WithOne("ScaleModel")
                        .HasForeignKey("FireSaverApi.DataContext.ScaleModel", "ApplyingEvacPlansId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("ApplyingEvacPlans");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.ScalePoint", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.ScaleModel", "ScaleModel")
                        .WithMany("ScalePoints")
                        .HasForeignKey("ScaleModelId");

                    b.Navigation("ScaleModel");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.User", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.Compartment", "CurrentCompartment")
                        .WithMany("InboundUsers")
                        .HasForeignKey("CurrentCompartmentId");

                    b.HasOne("FireSaverApi.DataContext.Building", "ResponsibleForBuilding")
                        .WithMany("ResponsibleUsers")
                        .HasForeignKey("ResponsibleForBuildingId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("CurrentCompartment");

                    b.Navigation("ResponsibleForBuilding");
                });

            modelBuilder.Entity("UserUserRole", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.UserRole", null)
                        .WithMany()
                        .HasForeignKey("RolesListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FireSaverApi.DataContext.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Floor", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.Building", "BuildingWithThisFloor")
                        .WithMany("Floors")
                        .HasForeignKey("BuildingWithThisFloorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("BuildingWithThisFloor");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Room", b =>
                {
                    b.HasOne("FireSaverApi.DataContext.Floor", "RoomFloor")
                        .WithMany("Rooms")
                        .HasForeignKey("RoomFloorId");

                    b.Navigation("RoomFloor");
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

                    b.Navigation("ScaleModel");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.RoutePoint", b =>
                {
                    b.Navigation("ChildrenPoints");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.ScaleModel", b =>
                {
                    b.Navigation("ScalePoints");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Test", b =>
                {
                    b.Navigation("Compartment");

                    b.Navigation("Questions");
                });

            modelBuilder.Entity("FireSaverApi.DataContext.Floor", b =>
                {
                    b.Navigation("Rooms");
                });
#pragma warning restore 612, 618
        }
    }
}
