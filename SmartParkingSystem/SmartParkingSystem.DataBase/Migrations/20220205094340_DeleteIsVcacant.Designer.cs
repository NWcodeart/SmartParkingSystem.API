﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartParkingSystem.DataBase.model;

namespace SmartParkingSystem.DataBase.Migrations
{
    [DbContext(typeof(ParkingContext))]
    [Migration("20220205094340_DeleteIsVcacant")]
    partial class DeleteIsVcacant
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SmartParkingSystem.DataBase.model.CompanyParking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("companyParkings");
                });

            modelBuilder.Entity("SmartParkingSystem.DataBase.model.ParkingSpace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CarNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsVacant")
                        .HasColumnType("bit");

                    b.Property<int>("ParkingId")
                        .HasColumnType("int");

                    b.Property<string>("ParkingNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ParkingId");

                    b.ToTable("parkingSpaces");
                });

            modelBuilder.Entity("SmartParkingSystem.DataBase.model.ParkingSpace", b =>
                {
                    b.HasOne("SmartParkingSystem.DataBase.model.CompanyParking", "companyParking")
                        .WithMany("ParkingList")
                        .HasForeignKey("ParkingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("companyParking");
                });

            modelBuilder.Entity("SmartParkingSystem.DataBase.model.CompanyParking", b =>
                {
                    b.Navigation("ParkingList");
                });
#pragma warning restore 612, 618
        }
    }
}
