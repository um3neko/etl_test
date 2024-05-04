﻿// <auto-generated />
using System;
using ETL_test.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ETL_test.Migrations
{
    [DbContext(typeof(ObjectModelDbContext))]
    [Migration("20240504011014_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ETL_test.Models.EFModels.ObjectModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DOLocationID")
                        .HasColumnType("int");

                    b.Property<float>("FareAmount")
                        .HasColumnType("real")
                        .HasColumnName("fare_amount");

                    b.Property<int>("PULocationID")
                        .HasColumnType("int");

                    b.Property<int>("PassengerCount")
                        .HasColumnType("int")
                        .HasColumnName("passenger_count");

                    b.Property<bool>("StoreAndFwdFlag")
                        .HasColumnType("bit")
                        .HasColumnName("store_and_fwd_flag");

                    b.Property<DateTime>("TPepDropoffDateTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("tpep_dropoff_datetime");

                    b.Property<DateTime>("TPepPickupDateTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("tpep_pickup_datetime");

                    b.Property<float>("TipAmount")
                        .HasColumnType("real")
                        .HasColumnName("tip_amount");

                    b.Property<float>("TripDistance")
                        .HasColumnType("real")
                        .HasColumnName("trip_distance");

                    b.HasKey("Id");

                    b.ToTable("ObjectModels");
                });
#pragma warning restore 612, 618
        }
    }
}
