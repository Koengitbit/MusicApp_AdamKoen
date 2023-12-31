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
    public DbSet<AlbumSong> AlbumSongs { get; set; }
    public DbSet<User> Users { get; set; }

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

        modelBuilder.Entity<User>()
        .HasMany(u => u.Playlists)
        .WithOne(p => p.User)
        .HasForeignKey(p => p.UserId);


        modelBuilder.Entity<Artist>().HasData(
            new Artist { Id = 1, Name = "Artist One" },
            new Artist { Id = 2, Name = "Artist Two" }
        );

        modelBuilder.Entity<Album>().HasData(
            new Album { Id = 1, Title = "Album One", Name = "Album One", Release_Year = new DateTime(2020, 1, 1) },
            new Album { Id = 2, Title = "Album Two", Name = "Album Two", Release_Year = new DateTime(2021, 1, 1) }
        );

        modelBuilder.Entity<Song>().HasData(
            new Song { Id = 1, Title = "Song One", Genre = "Pop", ReleaseDate = new DateTime(2020, 5, 1), Duration = 210, ArtistId = 1 },
            new Song { Id = 2, Title = "Song Two", Genre = "Rock", ReleaseDate = new DateTime(2021, 6, 1), Duration = 180, ArtistId = 2 }
        );

        modelBuilder.Entity<Playlist>().HasData(
            new Playlist { Id = 1, Name = "Playlist One", IsPublic = true, CreatedAt = new DateTime(2022, 1, 1) },
            new Playlist { Id = 2, Name = "Playlist Two", IsPublic = false, CreatedAt = new DateTime(2022, 2, 1) }
        );

        // For Many-to-Many relationship, you would need to add entries in the join table
        modelBuilder.Entity<PlaylistSong>().HasData(
            new PlaylistSong { PlaylistId = 1, SongId = 1 },
            new PlaylistSong { PlaylistId = 2, SongId = 2 }
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MusicApp_AdamKoenDb;Integrated Security=true;");
    }
}