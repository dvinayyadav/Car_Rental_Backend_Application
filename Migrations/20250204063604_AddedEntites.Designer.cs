﻿// <auto-generated />
using System;
using Car_Rental_Backend_Application.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Car_Rental_Backend_Application.Migrations
{
    [DbContext(typeof(CarRentalContext))]
    [Migration("20250204063604_AddedEntites")]
    partial class AddedEntites
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Car_Rental_Backend_Application.Data.Entities.Admin", b =>
                {
                    b.Property<int>("Admin_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Admin_ID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Admin_ID");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("Car_Rental_Backend_Application.Data.Entities.Availability", b =>
                {
                    b.Property<int>("Car_ID")
                        .HasColumnType("int");

                    b.Property<bool>("Available")
                        .HasColumnType("bit");

                    b.Property<DateTime>("Pickup_Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Return_Date")
                        .HasColumnType("datetime2");

                    b.HasKey("Car_ID");

                    b.ToTable("Availabilities");
                });

            modelBuilder.Entity("Car_Rental_Backend_Application.Data.Entities.Booking", b =>
                {
                    b.Property<int>("BookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingId"));

                    b.Property<int?>("Admin_ID")
                        .HasColumnType("int");

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Car_ID")
                        .HasColumnType("int");

                    b.Property<DateTime>("PickupDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("User_ID")
                        .HasColumnType("int");

                    b.HasKey("BookingId");

                    b.HasIndex("Admin_ID");

                    b.HasIndex("Car_ID");

                    b.HasIndex("User_ID");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("Car_Rental_Backend_Application.Data.Entities.Cancellation", b =>
                {
                    b.Property<int>("Cancellation_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Cancellation_ID"));

                    b.Property<int>("Booking_ID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Cancellation_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Cancellation_ID");

                    b.HasIndex("Booking_ID");

                    b.ToTable("Cancellations");
                });

            modelBuilder.Entity("Car_Rental_Backend_Application.Data.Entities.Car", b =>
                {
                    b.Property<int>("Car_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Car_ID"));

                    b.Property<string>("Availability_Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("License_Plate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Car_ID");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("Car_Rental_Backend_Application.Data.Entities.Reservation", b =>
                {
                    b.Property<int>("Reservation_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Reservation_ID"));

                    b.Property<int>("Car_ID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Pickup_Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Reservation_Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Return_Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("User_ID")
                        .HasColumnType("int");

                    b.HasKey("Reservation_ID");

                    b.HasIndex("Car_ID");

                    b.HasIndex("User_ID");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("Car_Rental_Backend_Application.Data.Entities.User", b =>
                {
                    b.Property<int>("User_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("User_ID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone_Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("User_ID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Phone_Number")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Car_Rental_Backend_Application.Data.Entities.Availability", b =>
                {
                    b.HasOne("Car_Rental_Backend_Application.Data.Entities.Car", "Car")
                        .WithOne("Availability")
                        .HasForeignKey("Car_Rental_Backend_Application.Data.Entities.Availability", "Car_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");
                });

            modelBuilder.Entity("Car_Rental_Backend_Application.Data.Entities.Booking", b =>
                {
                    b.HasOne("Car_Rental_Backend_Application.Data.Entities.Admin", null)
                        .WithMany("Bookings")
                        .HasForeignKey("Admin_ID");

                    b.HasOne("Car_Rental_Backend_Application.Data.Entities.Car", "Car")
                        .WithMany("Bookings")
                        .HasForeignKey("Car_ID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Car_Rental_Backend_Application.Data.Entities.User", "User")
                        .WithMany("Bookings")
                        .HasForeignKey("User_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Car_Rental_Backend_Application.Data.Entities.Cancellation", b =>
                {
                    b.HasOne("Car_Rental_Backend_Application.Data.Entities.Booking", "Booking")
                        .WithMany("Cancellations")
                        .HasForeignKey("Booking_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("Car_Rental_Backend_Application.Data.Entities.Reservation", b =>
                {
                    b.HasOne("Car_Rental_Backend_Application.Data.Entities.Car", "Car")
                        .WithMany("Reservations")
                        .HasForeignKey("Car_ID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Car_Rental_Backend_Application.Data.Entities.User", "User")
                        .WithMany("Reservations")
                        .HasForeignKey("User_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Car_Rental_Backend_Application.Data.Entities.Admin", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("Car_Rental_Backend_Application.Data.Entities.Booking", b =>
                {
                    b.Navigation("Cancellations");
                });

            modelBuilder.Entity("Car_Rental_Backend_Application.Data.Entities.Car", b =>
                {
                    b.Navigation("Availability")
                        .IsRequired();

                    b.Navigation("Bookings");

                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("Car_Rental_Backend_Application.Data.Entities.User", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
