﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieBooking.Persistence.Database;

#nullable disable

namespace MovieBooking.Persistence.Migrations
{
    [DbContext(typeof(MovieDBContext))]
    [Migration("20240311053946_CHangeMigration")]
    partial class CHangeMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MovieBooking.Domain.Entities.Booking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("DeletedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("SeatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SeatsBooked")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ShowtimeID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SeatId");

                    b.HasIndex("ShowtimeID");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BookingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("DeletedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.MovieBooking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("DeletedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Duration")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("movieBookings");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.PaymentMethod", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("DeletedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PaymentMethod");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.Screen", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("DeletedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("TheaterId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TheaterId");

                    b.ToTable("Screens");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.Seat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Column")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("DeletedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Row")
                        .HasColumnType("int");

                    b.Property<Guid>("ScreenId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ScreenId");

                    b.ToTable("seats");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.Showtime", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("DeletedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("MovieId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ScreenID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("MovieId");

                    b.HasIndex("ScreenID");

                    b.ToTable("showtimes");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.Theater", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("DeletedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NoOfScreen")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Theater");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("BookingId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("DeletedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("PaymentMethodID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BookingId");

                    b.HasIndex("PaymentMethodID");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.Booking", b =>
                {
                    b.HasOne("MovieBooking.Domain.Entities.Seat", null)
                        .WithMany("Bookings")
                        .HasForeignKey("SeatId");

                    b.HasOne("MovieBooking.Domain.Entities.Showtime", "Showtime")
                        .WithMany("Bookings")
                        .HasForeignKey("ShowtimeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Showtime");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.Movie", b =>
                {
                    b.HasOne("MovieBooking.Domain.Entities.Booking", null)
                        .WithMany("movies")
                        .HasForeignKey("BookingId");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.Screen", b =>
                {
                    b.HasOne("MovieBooking.Domain.Entities.Theater", "theater")
                        .WithMany("Screens")
                        .HasForeignKey("TheaterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("theater");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.Seat", b =>
                {
                    b.HasOne("MovieBooking.Domain.Entities.Screen", "screen")
                        .WithMany("Seats")
                        .HasForeignKey("ScreenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("screen");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.Showtime", b =>
                {
                    b.HasOne("MovieBooking.Domain.Entities.Movie", "Movie")
                        .WithMany("Showtimes")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieBooking.Domain.Entities.Screen", "Screen")
                        .WithMany("Showtimes")
                        .HasForeignKey("ScreenID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");

                    b.Navigation("Screen");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.Transaction", b =>
                {
                    b.HasOne("MovieBooking.Domain.Entities.Booking", "Booking")
                        .WithMany("Transactions")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieBooking.Domain.Entities.PaymentMethod", "PaymentMethod")
                        .WithMany("Transactions")
                        .HasForeignKey("PaymentMethodID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("PaymentMethod");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.Booking", b =>
                {
                    b.Navigation("Transactions");

                    b.Navigation("movies");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.Movie", b =>
                {
                    b.Navigation("Showtimes");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.PaymentMethod", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.Screen", b =>
                {
                    b.Navigation("Seats");

                    b.Navigation("Showtimes");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.Seat", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.Showtime", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("MovieBooking.Domain.Entities.Theater", b =>
                {
                    b.Navigation("Screens");
                });
#pragma warning restore 612, 618
        }
    }
}
