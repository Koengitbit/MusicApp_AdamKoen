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
    public DbSet<PlayHistory> PlayHistory { get; set; }

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
            new Song { Id = 9, Title = "P.Y.T (Pretty Young Thing)", Genre = "Pop", ReleaseDate = new DateTime(2020, 5, 1), Duration = 210, ArtistId = 1 },
            new Song { Id = 10, Title = "Ambitionz Az A Ridah", Genre = "Hip Hop", ReleaseDate = new DateTime(1996, 1,1), Duration = 278, ArtistId = 2 },
            new Song { Id = 11, Title = "All About U", Genre = "Hip Hop", ReleaseDate = new DateTime(1996, 1, 1), Duration = 276, ArtistId = 2 },
            new Song { Id = 12, Title = "Skandalouz", Genre = "Hip Hop", ReleaseDate = new DateTime(1996, 1, 1), Duration = 248, ArtistId = 2 },
            new Song { Id = 13, Title = "Got My Mind Made Up", Genre = "Hip Hop", ReleaseDate = new DateTime(1996, 1, 1), Duration = 312, ArtistId = 2 },
            new Song { Id = 14, Title = "How Do U Want It", Genre = "Hip Hop", ReleaseDate = new DateTime(1996, 1, 1), Duration = 285, ArtistId = 2 },
            new Song { Id = 15, Title = "2 Of Amerikaz Most Wanted", Genre = "Hip Hop", ReleaseDate = new DateTime(1996, 1, 1), Duration = 246, ArtistId = 2 },
            new Song { Id = 16, Title = "No More Pain", Genre = "Hip Hop", ReleaseDate = new DateTime(1996, 1, 1), Duration = 374, ArtistId = 2 },
            new Song { Id = 17, Title = "Life Goes On", Genre = "Hip Hop", ReleaseDate = new DateTime(1996, 1, 1), Duration = 283, ArtistId = 2 },
            new Song { Id = 18, Title = "Only God Can Judge Me", Genre = "Hip Hop", ReleaseDate = new DateTime(1996, 1, 1), Duration = 314, ArtistId = 2 },
            new Song { Id = 19, Title = "Tradin' War Stories", Genre = "Hip Hop", ReleaseDate = new DateTime(1996, 1, 1), Duration = 314, ArtistId = 2 }
            );
        modelBuilder.Entity<Playlist>().HasData(
            new Playlist { Id = 1, Name = "Playlist 1", IsPublic = true, CreatedAt = new DateTime(2022, 1, 1), UserId = 1 },
            new Playlist { Id = 2, Name = "Playlist 2", IsPublic = false, CreatedAt = new DateTime(2022, 2, 1), UserId = 1 }
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
           new AlbumSong { AlbumId = 1, SongId = 9, OrderIndex = 7 },
           new AlbumSong { AlbumId = 2, SongId = 10, OrderIndex = 0 },
           new AlbumSong { AlbumId = 2, SongId = 11, OrderIndex = 1 },
           new AlbumSong { AlbumId = 2, SongId = 12, OrderIndex = 2 },
           new AlbumSong { AlbumId = 2, SongId = 13, OrderIndex = 3 },
           new AlbumSong { AlbumId = 2, SongId = 14, OrderIndex = 4 },
           new AlbumSong { AlbumId = 2, SongId = 15, OrderIndex = 5 },
           new AlbumSong { AlbumId = 2, SongId = 16, OrderIndex = 6 },
           new AlbumSong { AlbumId = 2, SongId = 17, OrderIndex = 7 },
           new AlbumSong { AlbumId = 2, SongId = 18, OrderIndex = 8 },
           new AlbumSong { AlbumId = 2, SongId = 19, OrderIndex = 9 },
           new AlbumSong { AlbumId = 2, SongId = 2, OrderIndex = 10 }
       );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MusicApp_AdamKoenDb;Integrated Security=true;");
    }
}