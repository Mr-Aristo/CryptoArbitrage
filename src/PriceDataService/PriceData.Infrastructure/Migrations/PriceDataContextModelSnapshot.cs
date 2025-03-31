﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PriceData.Infrastructure.Data;

#nullable disable

namespace PriceData.Infrastructure.Migrations
{
    [DbContext(typeof(PriceDataContext))]
    partial class PriceDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PriceData.Domain.Models.FuturePrice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("FuturePrices");

                    b.HasData(
                        new
                        {
                            Id = new Guid("14afa7c6-602e-455d-9c4c-b113c0ba12ae"),
                            Price = 45000.50m,
                            Symbol = "BTCUSDT_QUARTER",
                            TimeStamp = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            Id = new Guid("e9a5085c-64dc-46e8-9383-4d083732e334"),
                            Price = 3000.25m,
                            Symbol = "BTCUSDT_BI-QUARTER",
                            TimeStamp = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
