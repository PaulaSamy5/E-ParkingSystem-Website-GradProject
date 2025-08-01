﻿// <auto-generated />
using System;
using GraduationProject_E_ParkingSystem.Models.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GraduationProject_E_ParkingSystem.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20250209193023_V504")]
    partial class V504
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GraduationProject_E_ParkingSystem.Models.ParkingRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Cost")
                        .HasColumnType("float");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("bit");

                    b.Property<int?>("ParkingSpotId")
                        .HasColumnType("int");

                    b.Property<string>("PlateNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QRCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("vehicleType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ParkingSpotId");

                    b.HasIndex("UserId");

                    b.ToTable("parkingRecords");
                });

            modelBuilder.Entity("GraduationProject_E_ParkingSystem.Models.ParkingSetting", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<double>("PricePerHour")
                        .HasColumnType("float");

                    b.HasKey("id");

                    b.ToTable("parkingSettings");
                });

            modelBuilder.Entity("GraduationProject_E_ParkingSystem.Models.Slot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<bool>("Isbooked")
                        .HasColumnType("bit");

                    b.Property<string>("SlotName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ParkingSpots");
                });

            modelBuilder.Entity("GraduationProject_E_ParkingSystem.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("LName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GraduationProject_E_ParkingSystem.Models.ParkingRecord", b =>
                {
                    b.HasOne("GraduationProject_E_ParkingSystem.Models.Slot", "ParkingSpot")
                        .WithMany("ParkingRecords")
                        .HasForeignKey("ParkingSpotId");

                    b.HasOne("GraduationProject_E_ParkingSystem.Models.User", "User")
                        .WithMany("ParkingRecords")
                        .HasForeignKey("UserId");

                    b.Navigation("ParkingSpot");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GraduationProject_E_ParkingSystem.Models.Slot", b =>
                {
                    b.Navigation("ParkingRecords");
                });

            modelBuilder.Entity("GraduationProject_E_ParkingSystem.Models.User", b =>
                {
                    b.Navigation("ParkingRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
