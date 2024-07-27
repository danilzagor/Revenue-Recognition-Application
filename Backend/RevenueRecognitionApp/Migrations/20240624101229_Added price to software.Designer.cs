﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RevenueRecognition.Contexts;

#nullable disable

namespace RevenueRecognition.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240624101229_Added price to software")]
    partial class Addedpricetosoftware
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.5.24306.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RevenueRecognition.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("RevenueRecognition.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("RevenueRecognition.Models.CompanyClient", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("KRS")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.ToTable("CompanyClients");
                });

            modelBuilder.Entity("RevenueRecognition.Models.Contract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ActualisationPeriod")
                        .HasColumnType("int");

                    b.Property<DateOnly>("BeginningDate")
                        .HasColumnType("date");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("EndingDate")
                        .HasColumnType("date");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("SoftwareAndVersionId")
                        .HasColumnType("int");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("SoftwareAndVersionId");

                    b.HasIndex("StatusId");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("RevenueRecognition.Models.ContractStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ContractStatuses");
                });

            modelBuilder.Entity("RevenueRecognition.Models.PhysicalClient", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<string>("PESEL")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.HasKey("Id");

                    b.ToTable("PhysicalClients");
                });

            modelBuilder.Entity("RevenueRecognition.Models.Sale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("EndAt")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<DateOnly>("StartAt")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("RevenueRecognition.Models.Software", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Software");
                });

            modelBuilder.Entity("RevenueRecognition.Models.SoftwareCategories", b =>
                {
                    b.Property<int>("SoftwareId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("SoftwareId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Software_Categories");
                });

            modelBuilder.Entity("RevenueRecognition.Models.SoftwareSales", b =>
                {
                    b.Property<int>("SoftwareId")
                        .HasColumnType("int");

                    b.Property<int>("SaleId")
                        .HasColumnType("int");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("SoftwareId", "SaleId");

                    b.HasIndex("SaleId");

                    b.ToTable("Software_Sales");
                });

            modelBuilder.Entity("RevenueRecognition.Models.SoftwareVersion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int>("SoftwareId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SoftwareId");

                    b.ToTable("SoftwareVersions");
                });

            modelBuilder.Entity("RevenueRecognition.Models.CompanyClient", b =>
                {
                    b.HasOne("RevenueRecognition.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("RevenueRecognition.Models.Contract", b =>
                {
                    b.HasOne("RevenueRecognition.Models.Client", "Client")
                        .WithMany("Contracts")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RevenueRecognition.Models.SoftwareVersion", "SoftwareVersion")
                        .WithMany()
                        .HasForeignKey("SoftwareAndVersionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RevenueRecognition.Models.ContractStatus", "ContractStatus")
                        .WithMany("Contracts")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("ContractStatus");

                    b.Navigation("SoftwareVersion");
                });

            modelBuilder.Entity("RevenueRecognition.Models.PhysicalClient", b =>
                {
                    b.HasOne("RevenueRecognition.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("RevenueRecognition.Models.SoftwareCategories", b =>
                {
                    b.HasOne("RevenueRecognition.Models.Category", "Category")
                        .WithMany("SoftwareCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RevenueRecognition.Models.Software", "Software")
                        .WithMany("SoftwareCategories")
                        .HasForeignKey("SoftwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Software");
                });

            modelBuilder.Entity("RevenueRecognition.Models.SoftwareSales", b =>
                {
                    b.HasOne("RevenueRecognition.Models.Sale", "Sale")
                        .WithMany()
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RevenueRecognition.Models.Software", "Software")
                        .WithMany("SoftwareSales")
                        .HasForeignKey("SoftwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sale");

                    b.Navigation("Software");
                });

            modelBuilder.Entity("RevenueRecognition.Models.SoftwareVersion", b =>
                {
                    b.HasOne("RevenueRecognition.Models.Software", "Software")
                        .WithMany("SoftwareVersions")
                        .HasForeignKey("SoftwareId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Software");
                });

            modelBuilder.Entity("RevenueRecognition.Models.Category", b =>
                {
                    b.Navigation("SoftwareCategories");
                });

            modelBuilder.Entity("RevenueRecognition.Models.Client", b =>
                {
                    b.Navigation("Contracts");
                });

            modelBuilder.Entity("RevenueRecognition.Models.ContractStatus", b =>
                {
                    b.Navigation("Contracts");
                });

            modelBuilder.Entity("RevenueRecognition.Models.Software", b =>
                {
                    b.Navigation("SoftwareCategories");

                    b.Navigation("SoftwareSales");

                    b.Navigation("SoftwareVersions");
                });
#pragma warning restore 612, 618
        }
    }
}
