﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SAKnowledgeBase.DataBase;

#nullable disable

namespace SAKnowledgeBase.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241128082701_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SAKnowledgeBase.DataBase.Entities.Info", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FormatId")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<int>("SequenceNum")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FormatId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Infos");
                });

            modelBuilder.Entity("SAKnowledgeBase.DataBase.Entities.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("QuestionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SequenceNum")
                        .HasColumnType("int");

                    b.Property<int>("ThemeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ThemeId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("SAKnowledgeBase.DataBase.Entities.TextFormat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Bold")
                        .HasColumnType("bit");

                    b.Property<string>("FormatName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TextSize")
                        .HasColumnType("int");

                    b.Property<bool>("Tilt")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Formats");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Bold = true,
                            FormatName = "Раздел",
                            TextSize = 7,
                            Tilt = false
                        },
                        new
                        {
                            Id = 2,
                            Bold = true,
                            FormatName = "Вопрос",
                            TextSize = 6,
                            Tilt = false
                        },
                        new
                        {
                            Id = 3,
                            Bold = true,
                            FormatName = "Подраздел",
                            TextSize = 5,
                            Tilt = false
                        },
                        new
                        {
                            Id = 4,
                            Bold = false,
                            FormatName = "Текст",
                            TextSize = 4,
                            Tilt = false
                        },
                        new
                        {
                            Id = 5,
                            Bold = true,
                            FormatName = "Текст жирный",
                            TextSize = 4,
                            Tilt = false
                        },
                        new
                        {
                            Id = 6,
                            Bold = false,
                            FormatName = "Перечисления",
                            TextSize = 4,
                            Tilt = false
                        },
                        new
                        {
                            Id = 7,
                            Bold = false,
                            FormatName = "Пояснение",
                            TextSize = 3,
                            Tilt = true
                        });
                });

            modelBuilder.Entity("SAKnowledgeBase.DataBase.Entities.Theme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("SequenceNum")
                        .HasColumnType("int");

                    b.Property<string>("ThemeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Themes");
                });

            modelBuilder.Entity("SAKnowledgeBase.DataBase.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "ad@min.admin",
                            Password = new byte[] { 30, 219, 129, 122, 145, 126, 234, 137, 47, 81, 73, 6, 233, 150, 77, 115, 175, 223, 43, 69, 243, 84, 173, 191, 17, 19, 116, 148, 86, 217, 82, 174, 205, 250, 63, 72, 96, 2, 81, 63, 153, 136, 235, 45, 170, 55, 45, 47, 232, 38, 55, 156, 182, 151, 54, 238, 4, 62, 44, 217, 58, 217, 91, 187 },
                            Role = 0,
                            Salt = new byte[] { 122, 42, 234, 176, 20, 57, 58, 186, 170, 167, 236, 134, 87, 224, 140, 65 }
                        });
                });

            modelBuilder.Entity("SAKnowledgeBase.DataBase.Entities.Info", b =>
                {
                    b.HasOne("SAKnowledgeBase.DataBase.Entities.TextFormat", "TextFormat")
                        .WithMany("Infos")
                        .HasForeignKey("FormatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SAKnowledgeBase.DataBase.Entities.Question", "Question")
                        .WithMany("Infos")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("TextFormat");
                });

            modelBuilder.Entity("SAKnowledgeBase.DataBase.Entities.Question", b =>
                {
                    b.HasOne("SAKnowledgeBase.DataBase.Entities.Theme", "Theme")
                        .WithMany("Questions")
                        .HasForeignKey("ThemeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Theme");
                });

            modelBuilder.Entity("SAKnowledgeBase.DataBase.Entities.Question", b =>
                {
                    b.Navigation("Infos");
                });

            modelBuilder.Entity("SAKnowledgeBase.DataBase.Entities.TextFormat", b =>
                {
                    b.Navigation("Infos");
                });

            modelBuilder.Entity("SAKnowledgeBase.DataBase.Entities.Theme", b =>
                {
                    b.Navigation("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}