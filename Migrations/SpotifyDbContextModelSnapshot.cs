﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicApp_AdamKoen.DAL;

#nullable disable

namespace MusicAppAdamKoen.Migrations
{
    [DbContext(typeof(SpotifyDbContext))]
    partial class SpotifyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MusicApp_AdamKoen.Models.Album", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Release_Year")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Albums");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ReleaseYear = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Thriller 25 Super Deluxe Edition"
                        },
                        new
                        {
                            Id = 2,
                            ReleaseYear = new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "All Eyez On Me"
                        });
                });

            modelBuilder.Entity("MusicApp_AdamKoen.Models.AlbumSong", b =>
                {
                    b.Property<int>("AlbumId")
                        .HasColumnType("int");

                    b.Property<int>("SongId")
                        .HasColumnType("int");

                    b.HasKey("AlbumId", "SongId");

                    b.HasIndex("SongId");

                    b.ToTable("AlbumSongs");
                });

            modelBuilder.Entity("MusicApp_AdamKoen.Models.Artist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Artists");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Michael Jackson"
                        },
                        new
                        {
                            Id = 2,
                            Name = "2Pac"
                        });
                });

            modelBuilder.Entity("MusicApp_AdamKoen.Models.Playlist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Playlists");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsPublic = true,
                            Name = "Playlist One",
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2022, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsPublic = false,
                            Name = "Playlist Two",
                            UserId = 1
                        });
                });

            modelBuilder.Entity("MusicApp_AdamKoen.Models.PlaylistSong", b =>
                {
                    b.Property<int>("PlaylistId")
                        .HasColumnType("int");

                    b.Property<int>("SongId")
                        .HasColumnType("int");

                    b.HasKey("PlaylistId", "SongId");

                    b.HasIndex("SongId");

                    b.ToTable("PlaylistSongs");

                    b.HasData(
                        new
                        {
                            PlaylistId = 1,
                            SongId = 1
                        },
                        new
                        {
                            PlaylistId = 2,
                            SongId = 2
                        });
                });

            modelBuilder.Entity("MusicApp_AdamKoen.Models.Song", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ArtistId")
                        .HasColumnType("int");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.ToTable("Songs");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ArtistId = 1,
                            Duration = 362,
                            Genre = "Pop",
                            ReleaseDate = new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Wanna Be Startin' Somethin'"
                        },
                        new
                        {
                            Id = 2,
                            ArtistId = 2,
                            Duration = 180,
                            Genre = "Hip Hop",
                            ReleaseDate = new DateTime(2021, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Changes"
                        },
                        new
                        {
                            Id = 3,
                            ArtistId = 1,
                            Duration = 260,
                            Genre = "Pop",
                            ReleaseDate = new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Baby Be Mine"
                        },
                        new
                        {
                            Id = 4,
                            ArtistId = 1,
                            Duration = 222,
                            Genre = "Pop",
                            ReleaseDate = new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "The Girl Is Mine"
                        },
                        new
                        {
                            Id = 5,
                            ArtistId = 1,
                            Duration = 210,
                            Genre = "Pop",
                            ReleaseDate = new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Thriller"
                        },
                        new
                        {
                            Id = 6,
                            ArtistId = 1,
                            Duration = 210,
                            Genre = "Pop",
                            ReleaseDate = new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Beat it"
                        },
                        new
                        {
                            Id = 7,
                            ArtistId = 1,
                            Duration = 210,
                            Genre = "Pop",
                            ReleaseDate = new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Billie Jean"
                        },
                        new
                        {
                            Id = 8,
                            ArtistId = 1,
                            Duration = 210,
                            Genre = "Pop",
                            ReleaseDate = new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Human Nature"
                        },
                        new
                        {
                            Id = 9,
                            ArtistId = 1,
                            Duration = 210,
                            Genre = "Pop",
                            ReleaseDate = new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "P.Y.T (Pretty Young Thing)"
                        });
                });

            modelBuilder.Entity("MusicApp_AdamKoen.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BirthDate = new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "testuser@example.com",
                            Name = "Test User",
                            Password = "hashedpassword"
                        });
                });

            modelBuilder.Entity("MusicApp_AdamKoen.Models.AlbumSong", b =>
                {
                    b.HasOne("MusicApp_AdamKoen.Models.Album", "Album")
                        .WithMany("Songs")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicApp_AdamKoen.Models.Song", "Song")
                        .WithMany("Albums")
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Album");

                    b.Navigation("Song");
                });

            modelBuilder.Entity("MusicApp_AdamKoen.Models.Playlist", b =>
                {
                    b.HasOne("MusicApp_AdamKoen.Models.User", "User")
                        .WithMany("Playlists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MusicApp_AdamKoen.Models.PlaylistSong", b =>
                {
                    b.HasOne("MusicApp_AdamKoen.Models.Playlist", "Playlist")
                        .WithMany("Songs")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MusicApp_AdamKoen.Models.Song", "Song")
                        .WithMany("Playlists")
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Playlist");

                    b.Navigation("Song");
                });

            modelBuilder.Entity("MusicApp_AdamKoen.Models.Song", b =>
                {
                    b.HasOne("MusicApp_AdamKoen.Models.Artist", "Artist")
                        .WithMany("Songs")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");
                });

            modelBuilder.Entity("MusicApp_AdamKoen.Models.Album", b =>
                {
                    b.Navigation("Songs");
                });

            modelBuilder.Entity("MusicApp_AdamKoen.Models.Artist", b =>
                {
                    b.Navigation("Songs");
                });

            modelBuilder.Entity("MusicApp_AdamKoen.Models.Playlist", b =>
                {
                    b.Navigation("Songs");
                });

            modelBuilder.Entity("MusicApp_AdamKoen.Models.Song", b =>
                {
                    b.Navigation("Albums");

                    b.Navigation("Playlists");
                });

            modelBuilder.Entity("MusicApp_AdamKoen.Models.User", b =>
                {
                    b.Navigation("Playlists");
                });
#pragma warning restore 612, 618
        }
    }
}
