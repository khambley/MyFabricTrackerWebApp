﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyFabricTrackerWebApp.Models;

namespace MyFabricTrackerWebApp.Migrations
{
    [DbContext(typeof(FabricTrackerDbContext))]
    [Migration("20210406132248_AddedBrandDesignerToFabricsModel")]
    partial class AddedBrandDesignerToFabricsModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyFabricTrackerWebApp.Models.Fabric", b =>
                {
                    b.Property<long>("FabricID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccentColor1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AccentColor2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AccentColor3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BackgroundColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Brand")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Designer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FabricItemCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FabricName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FabricNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FabricTypeId")
                        .HasColumnType("int");

                    b.Property<string>("FabricWidth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("FatQtrQty")
                        .HasColumnType("bigint");

                    b.Property<string>("ImageFileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDiscontinued")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPopular")
                        .HasColumnType("bit");

                    b.Property<long>("MainCategoryId")
                        .HasColumnType("bigint");

                    b.Property<int?>("SourceId")
                        .HasColumnType("int");

                    b.Property<long>("SubCategoryId")
                        .HasColumnType("bigint");

                    b.Property<long?>("TotalInches")
                        .HasColumnType("bigint");

                    b.HasKey("FabricID");

                    b.HasIndex("FabricTypeId");

                    b.HasIndex("MainCategoryId");

                    b.HasIndex("SourceId");

                    b.HasIndex("SubCategoryId");

                    b.ToTable("Fabrics");
                });

            modelBuilder.Entity("MyFabricTrackerWebApp.Models.FabricType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FabricType");
                });

            modelBuilder.Entity("MyFabricTrackerWebApp.Models.MainCategory", b =>
                {
                    b.Property<long>("MainCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("MainCategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MainCategoryId");

                    b.ToTable("MainCategories");
                });

            modelBuilder.Entity("MyFabricTrackerWebApp.Models.Source", b =>
                {
                    b.Property<int>("SourceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SourceName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WebsiteUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SourceId");

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("MyFabricTrackerWebApp.Models.SubCategory", b =>
                {
                    b.Property<long>("SubCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("MainCategoryId")
                        .HasColumnType("bigint");

                    b.Property<string>("SubCategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SubCategoryId");

                    b.HasIndex("MainCategoryId");

                    b.ToTable("SubCategories");
                });

            modelBuilder.Entity("MyFabricTrackerWebApp.Models.Transaction", b =>
                {
                    b.Property<long>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("FabricId")
                        .HasColumnType("bigint");

                    b.Property<int?>("FatQtrQty")
                        .HasColumnType("int");

                    b.Property<int?>("InchesQty")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("TransactionId");

                    b.HasIndex("FabricId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("MyFabricTrackerWebApp.Models.Fabric", b =>
                {
                    b.HasOne("MyFabricTrackerWebApp.Models.FabricType", "FabricType")
                        .WithMany()
                        .HasForeignKey("FabricTypeId");

                    b.HasOne("MyFabricTrackerWebApp.Models.MainCategory", "MainCategory")
                        .WithMany()
                        .HasForeignKey("MainCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyFabricTrackerWebApp.Models.Source", "Source")
                        .WithMany()
                        .HasForeignKey("SourceId");

                    b.HasOne("MyFabricTrackerWebApp.Models.SubCategory", "SubCategory")
                        .WithMany()
                        .HasForeignKey("SubCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FabricType");

                    b.Navigation("MainCategory");

                    b.Navigation("Source");

                    b.Navigation("SubCategory");
                });

            modelBuilder.Entity("MyFabricTrackerWebApp.Models.SubCategory", b =>
                {
                    b.HasOne("MyFabricTrackerWebApp.Models.MainCategory", "MainCategory")
                        .WithMany("SubCategories")
                        .HasForeignKey("MainCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MainCategory");
                });

            modelBuilder.Entity("MyFabricTrackerWebApp.Models.Transaction", b =>
                {
                    b.HasOne("MyFabricTrackerWebApp.Models.Fabric", "Fabric")
                        .WithMany()
                        .HasForeignKey("FabricId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Fabric");
                });

            modelBuilder.Entity("MyFabricTrackerWebApp.Models.MainCategory", b =>
                {
                    b.Navigation("SubCategories");
                });
#pragma warning restore 612, 618
        }
    }
}
