using Microsoft.EntityFrameworkCore;
using MusicApp_AdamKoen.Models;


namespace MusicApp_AdamKoen.DAL;

public class SpotifyDbContext : DbContext
{
    public SpotifyDbContext()
    {
    }

    public DbSet<Album> Albums { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<PlaylistSong> PlaylistSongs { get; set; }
    public DbSet<AlbumSong> AlbumSongs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Like> Likes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Song>()
            .HasOne(s => s.Artist)
            .WithMany(a => a.Songs)
            .HasForeignKey(s => s.ArtistId);

        // Many-to-Many: Song to Playlist relationship
        modelBuilder.Entity<PlaylistSong>()
            .HasKey(ps => new { ps.PlaylistId, ps.SongId });

        modelBuilder.Entity<PlaylistSong>()
            .HasOne(ps => ps.Playlist)
            .WithMany(p => p.Songs)
            .HasForeignKey(ps => ps.PlaylistId);

        modelBuilder.Entity<PlaylistSong>()
            .HasOne(ps => ps.Song)
            .WithMany(s => s.Playlists)
            .HasForeignKey(ps => ps.SongId);

        // Many-to-Many: Album to Song relationship
        modelBuilder.Entity<AlbumSong>()
            .HasKey(albumSong => new { albumSong.AlbumId, albumSong.SongId });

        modelBuilder.Entity<AlbumSong>()
            .HasOne(albumSong => albumSong.Album)
            .WithMany(a => a.Songs)
            .HasForeignKey(albumSong => albumSong.AlbumId);

        modelBuilder.Entity<AlbumSong>()
            .HasOne(albumSong => albumSong.Song)
            .WithMany(s => s.Albums)
            .HasForeignKey(albumSong => albumSong.SongId);

        modelBuilder.Entity<User>().HasData(
             new User
             {
                 Id = 1,
                 Name = "Test User",
                 Email = "testuser@example.com",
                 Password = "hashedpassword",
                 BirthDate = new DateTime(1990, 1, 1)
             }
         );

        modelBuilder.Entity<Artist>().HasData(
            new Artist { Id = 1, Name = "Michael Jackson" },
            new Artist { Id = 2, Name = "2Pac" }
        );

        modelBuilder.Entity<Album>().HasData(
            new Album { Id = 1, Title = "Thriller 25 Super Deluxe Edition", Release_Year = new DateTime(2020, 1, 1) },
            new Album { Id = 2, Title = "All Eyez On Me", Release_Year = new DateTime(2021, 1, 1) }
        );

        modelBuilder.Entity<Song>().HasData(
            new Song { Id = 1, Title = "Wanna Be Startin' Somethin'", Genre = "Pop", ReleaseDate = new DateTime(2020, 5, 1), Duration = 362, ArtistId = 1 },
            new Song { Id = 2, Title = "Changes", Genre = "Hip Hop", ReleaseDate = new DateTime(2021, 6, 1), Duration = 180, ArtistId = 2 },
            new Song { Id = 3, Title = "Baby Be Mine", Genre = "Pop", ReleaseDate = new DateTime(2020, 5, 1), Duration = 260, ArtistId = 1 },
            new Song { Id = 4, Title = "The Girl Is Mine", Genre = "Pop", ReleaseDate = new DateTime(2020, 5, 1), Duration = 222, ArtistId = 1 },
            new Song { Id = 5, Title = "Thriller", Genre = "Pop", ReleaseDate = new DateTime(2020, 5, 1), Duration = 210, ArtistId = 1 },
            new Song { Id = 6, Title = "Beat it", Genre = "Pop", ReleaseDate = new DateTime(2020, 5, 1), Duration = 210, ArtistId = 1 },
            new Song { Id = 7, Title = "Billie Jean", Genre = "Pop", ReleaseDate = new DateTime(2020, 5, 1), Duration = 210, ArtistId = 1 },
            new Song { Id = 8, Title = "Human Nature", Genre = "Pop", ReleaseDate = new DateTime(2020, 5, 1), Duration = 210, ArtistId = 1 },
            new Song { Id = 9, Title = "P.Y.T (Pretty Young Thing)", Genre = "Pop", ReleaseDate = new DateTime(2020, 5, 1), Duration = 210, ArtistId = 1 }
        );
        modelBuilder.Entity<Playlist>().HasData(
            new Playlist { Id = 1, Name = "Playlist One", IsPublic = true, CreatedAt = new DateTime(2022, 1, 1), UserId = 1 },
            new Playlist { Id = 2, Name = "Playlist Two", IsPublic = false, CreatedAt = new DateTime(2022, 2, 1), UserId = 1 }
        );

        modelBuilder.Entity<PlaylistSong>().HasData(
            new PlaylistSong { PlaylistId = 1, SongId = 1,OrderIndex = 2},
            new PlaylistSong { PlaylistId = 1, SongId = 2,OrderIndex = 1}
        );
        modelBuilder.Entity<AlbumSong>().HasData(
           new AlbumSong { AlbumId = 1, SongId = 1, OrderIndex = 0 },
           new AlbumSong { AlbumId = 1, SongId = 3, OrderIndex = 1 },
           new AlbumSong { AlbumId = 1, SongId = 4, OrderIndex = 2 },
           new AlbumSong { AlbumId = 1, SongId = 5, OrderIndex = 3 },
           new AlbumSong { AlbumId = 1, SongId = 6, OrderIndex = 4 },
           new AlbumSong { AlbumId = 1, SongId = 7, OrderIndex = 5 },
           new AlbumSong { AlbumId = 1, SongId = 8, OrderIndex = 6 },
           new AlbumSong { AlbumId = 1, SongId = 9, OrderIndex = 7 }

       );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MusicApp_AdamKoenDb;Integrated Security=true;");
    }
}