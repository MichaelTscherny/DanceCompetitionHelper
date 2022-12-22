﻿// <auto-generated />
using System;
using DanceCompetitionHelper.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DanceCompetitionHelper.Database.Migrations
{
    [DbContext(typeof(DanceCompetitionHelperDbContext))]
    [Migration("20221222170455_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.1");

            modelBuilder.Entity("DanceCompetitionHelper.Database.Tables.Competition", b =>
                {
                    b.Property<Guid>("CompetitionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CompetitionDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("CompetitionInfo")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("CompetitionName")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT")
                        .HasComment("Row created at (UTC)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT")
                        .HasComment("Row created by");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("TEXT")
                        .HasComment("Row last modified at (UTC)");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT")
                        .HasComment("Row last modified by");

                    b.Property<string>("OrgCompetitionId")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("TEXT")
                        .HasComment("'Internal' Org-Id of Competition");

                    b.Property<int>("Organization")
                        .HasColumnType("INTEGER");

                    b.HasKey("CompetitionId");

                    b.HasIndex("Organization", "OrgCompetitionId")
                        .IsUnique();

                    b.ToTable("Competitions", t =>
                        {
                            t.HasComment("A Competition 'root'");
                        });
                });

            modelBuilder.Entity("DanceCompetitionHelper.Database.Tables.CompetitionClass", b =>
                {
                    b.Property<Guid>("CompetitionClassId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("AgeClass")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<string>("AgeGroup")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<string>("Class")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CompetitionId")
                        .HasColumnType("TEXT")
                        .HasComment("Ref to Competition");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT")
                        .HasComment("Row created at (UTC)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT")
                        .HasComment("Row created by");

                    b.Property<string>("Discipline")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("TEXT")
                        .HasComment("Row last modified at (UTC)");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT")
                        .HasComment("Row last modified by");

                    b.Property<int>("MinPointsForPromotion")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MinStartsForPromotion")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OrgClassId")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("TEXT")
                        .HasComment("'Internal' Org-Id of class of CompetitionClass");

                    b.HasKey("CompetitionClassId");

                    b.HasIndex("CompetitionId", "OrgClassId")
                        .IsUnique();

                    b.ToTable("CompetitionClasses", t =>
                        {
                            t.HasComment("The classes of a Competition");
                        });
                });

            modelBuilder.Entity("DanceCompetitionHelper.Database.Tables.Participant", b =>
                {
                    b.Property<Guid>("ParticipantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CompetitionClassId")
                        .HasColumnType("TEXT")
                        .HasComment("Ref to CompetitionClass");

                    b.Property<Guid>("CompetitionId")
                        .HasColumnType("TEXT")
                        .HasComment("Ref to Competition");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT")
                        .HasComment("Row created at (UTC)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT")
                        .HasComment("Row created by");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("TEXT")
                        .HasComment("Row last modified at (UTC)");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT")
                        .HasComment("Row last modified by");

                    b.Property<string>("NamePartA")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NamePartB")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<string>("OrgIdClub")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<string>("OrgIdPartA")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("TEXT")
                        .HasComment("'Internal' Org-Id of class of CompetitionClass");

                    b.Property<string>("OrgIdPartB")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<int>("OrgPointsPartA")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("OrgPointsPartB")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrgStartsPartA")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("OrgStartsPartB")
                        .HasColumnType("INTEGER");

                    b.HasKey("ParticipantId");

                    b.HasIndex("CompetitionClassId");

                    b.HasIndex("CompetitionId");

                    b.ToTable("Participants", t =>
                        {
                            t.HasComment("The Participants of a Competition");
                        });
                });

            modelBuilder.Entity("DanceCompetitionHelper.Database.Tables.CompetitionClass", b =>
                {
                    b.HasOne("DanceCompetitionHelper.Database.Tables.Competition", "Competition")
                        .WithMany()
                        .HasForeignKey("CompetitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Competition");
                });

            modelBuilder.Entity("DanceCompetitionHelper.Database.Tables.Participant", b =>
                {
                    b.HasOne("DanceCompetitionHelper.Database.Tables.CompetitionClass", "CompetitionClass")
                        .WithMany()
                        .HasForeignKey("CompetitionClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DanceCompetitionHelper.Database.Tables.Competition", "Competition")
                        .WithMany()
                        .HasForeignKey("CompetitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Competition");

                    b.Navigation("CompetitionClass");
                });
#pragma warning restore 612, 618
        }
    }
}
