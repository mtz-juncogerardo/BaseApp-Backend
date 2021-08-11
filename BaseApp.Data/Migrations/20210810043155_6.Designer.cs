﻿// <auto-generated />
using System;
using BaseApp.Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BaseApp.Data.Migrations
{
    [DbContext(typeof(AuthenticationContext))]
    [Migration("20210810043155_6")]
    partial class _6
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("BaseApp.Data.DbModels.ArticleDbModel", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("AuditDbModelId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<decimal>("Discount")
                        .HasColumnType("numeric");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Total")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("AuditDbModelId")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Article");
                });

            modelBuilder.Entity("BaseApp.Data.DbModels.AuditDbModel", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<DateTime>("CreatedAtDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedByUserEmail")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("LastTouchedByIp")
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)");

                    b.Property<DateTime>("UpdatedAtDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UpdatedByUserEmail")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Audit");
                });

            modelBuilder.Entity("BaseApp.Data.DbModels.AuthenticationDbModel", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("EmailValid")
                        .HasColumnType("boolean");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("VersionId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.HasKey("Id");

                    b.ToTable("Authentication");
                });

            modelBuilder.Entity("BaseApp.Data.DbModels.UserDbModel", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("Address")
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)");

                    b.Property<string>("AuditDbModelId")
                        .IsRequired()
                        .HasColumnType("character varying(36)");

                    b.Property<string>("AuthenticationDbModelId")
                        .IsRequired()
                        .HasColumnType("character varying(36)");

                    b.Property<string>("City")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Name")
                        .HasMaxLength(36)
                        .HasColumnType("character varying(36)");

                    b.Property<string>("Phone")
                        .HasMaxLength(12)
                        .HasColumnType("character varying(12)");

                    b.Property<string>("PostalCode")
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<string>("State")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("AuditDbModelId")
                        .IsUnique();

                    b.HasIndex("AuthenticationDbModelId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BaseApp.Data.DbModels.ArticleDbModel", b =>
                {
                    b.HasOne("BaseApp.Data.DbModels.AuditDbModel", "AuditDbModel")
                        .WithOne("ArticleDbModel")
                        .HasForeignKey("BaseApp.Data.DbModels.ArticleDbModel", "AuditDbModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AuditDbModel");
                });

            modelBuilder.Entity("BaseApp.Data.DbModels.UserDbModel", b =>
                {
                    b.HasOne("BaseApp.Data.DbModels.AuditDbModel", "AuditDbModel")
                        .WithOne("UserDbModel")
                        .HasForeignKey("BaseApp.Data.DbModels.UserDbModel", "AuditDbModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BaseApp.Data.DbModels.AuthenticationDbModel", "AuthenticationDbModel")
                        .WithOne("UserDbModel")
                        .HasForeignKey("BaseApp.Data.DbModels.UserDbModel", "AuthenticationDbModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AuditDbModel");

                    b.Navigation("AuthenticationDbModel");
                });

            modelBuilder.Entity("BaseApp.Data.DbModels.AuditDbModel", b =>
                {
                    b.Navigation("ArticleDbModel");

                    b.Navigation("UserDbModel");
                });

            modelBuilder.Entity("BaseApp.Data.DbModels.AuthenticationDbModel", b =>
                {
                    b.Navigation("UserDbModel");
                });
#pragma warning restore 612, 618
        }
    }
}