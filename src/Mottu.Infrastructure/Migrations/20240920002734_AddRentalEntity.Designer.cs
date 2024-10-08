﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mottu.App.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240920002734_AddRentalEntity")]
    partial class AddRentalEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.33")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Mottu.App.DbContext.Models.Deliverer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Cnpj")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("character varying(14)");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Identificador")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImagemCnh")
                        .HasColumnType("text");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("NumeroCnh")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("TipoCnh")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.HasKey("Id");

                    b.HasIndex("Cnpj")
                        .IsUnique();

                    b.HasIndex("Identificador")
                        .IsUnique();

                    b.HasIndex("NumeroCnh")
                        .IsUnique();

                    b.ToTable("Deliverers");
                });

            modelBuilder.Entity("Mottu.App.DbContext.Models.Motorcycle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Ano")
                        .HasColumnType("integer");

                    b.Property<string>("Identificador")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Modelo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Identificador")
                        .IsUnique();

                    b.HasIndex("Placa")
                        .IsUnique();

                    b.ToTable("Motorcycles");
                });

            modelBuilder.Entity("Mottu.App.DbContext.Models.Rental", b =>
                {
                    b.Property<string>("Identificador")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DataDevolucao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DataInicio")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DataPrevisaoTermino")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DataTermino")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EntregadorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MotoId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Plano")
                        .HasColumnType("integer");

                    b.HasKey("Identificador");

                    b.ToTable("Rentals");

                    b.HasCheckConstraint("CK_Plano", "\"Plano\" IN (7, 15, 30, 45, 50)");
                });
#pragma warning restore 612, 618
        }
    }
}
