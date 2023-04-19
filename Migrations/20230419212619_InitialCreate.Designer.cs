﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace vtaWebAPI.Migrations
{
    [DbContext(typeof(TripUpdatesContext))]
    [Migration("20230419212619_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("StopTimeUpdate", b =>
                {
                    b.Property<string>("TripUpdateId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ArrivalTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("ScheduleRelationship")
                        .HasColumnType("TEXT");

                    b.Property<string>("StopId")
                        .HasColumnType("TEXT");

                    b.Property<int>("StopSequence")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TripUpdateId1")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("TripUpdateId");

                    b.HasIndex("TripUpdateId1");

                    b.ToTable("StopTimeUpdates");
                });

            modelBuilder.Entity("Trip", b =>
                {
                    b.Property<string>("TripId")
                        .HasColumnType("TEXT");

                    b.Property<int>("DirectionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RouteId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ScheduleRelationship")
                        .HasColumnType("TEXT");

                    b.Property<string>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("StartTime")
                        .HasColumnType("TEXT");

                    b.HasKey("TripId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("TripUpdate", b =>
                {
                    b.Property<string>("TripUpdateId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("TripId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("VehicleId")
                        .HasColumnType("TEXT");

                    b.HasKey("TripUpdateId");

                    b.ToTable("TripUpdates");
                });

            modelBuilder.Entity("StopTimeUpdate", b =>
                {
                    b.HasOne("TripUpdate", null)
                        .WithMany("StopTimeUpdates")
                        .HasForeignKey("TripUpdateId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TripUpdate", b =>
                {
                    b.Navigation("StopTimeUpdates");
                });
#pragma warning restore 612, 618
        }
    }
}
