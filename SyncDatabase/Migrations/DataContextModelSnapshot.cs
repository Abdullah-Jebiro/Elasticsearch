﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace SyncDatabase.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Patients");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Abdullah",
                            Gender = 1,
                            IsDeleted = false,
                            LastName = "Doe",
                            Region = "Region 1"
                        },
                        new
                        {
                            Id = 2,
                            DateOfBirth = new DateTime(1985, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Omar",
                            Gender = 2,
                            IsDeleted = false,
                            LastName = "Doe",
                            Region = "Region 2"
                        },
                        new
                        {
                            Id = 3,
                            DateOfBirth = new DateTime(1978, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "John",
                            Gender = 1,
                            IsDeleted = false,
                            LastName = "Smith",
                            Region = "Region 3"
                        },
                        new
                        {
                            Id = 4,
                            DateOfBirth = new DateTime(1992, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Emily",
                            Gender = 2,
                            IsDeleted = false,
                            LastName = "Johnson",
                            Region = "Region 4"
                        },
                        new
                        {
                            Id = 5,
                            DateOfBirth = new DateTime(1980, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Michael",
                            Gender = 1,
                            IsDeleted = false,
                            LastName = "Brown",
                            Region = "Region 5"
                        });
                });

            modelBuilder.Entity("UpdateEntitie", b =>
                {
                    b.Property<int>("EntityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EntityId"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("TypeEntities")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EntityId");

                    b.ToTable("UpdateEntities");
                });
#pragma warning restore 612, 618
        }
    }
}