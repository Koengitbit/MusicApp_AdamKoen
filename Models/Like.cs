using MusicApp_AdamKoen.Models;

public class Like
{
    public int Id { get; set; }

    public int UserId { get; set; } 
    public User User { get; set; }

    public int? SongId { get; set; }
    public Song? Song { get; set; }

    public int? AlbumId { get; set; }
    public Album? Album { get; set; }

    public int? ArtistId { get; set; }
    public Artist? Artist { get; set; }

    public int? PlaylistId { get; set; }
    public Playlist? Playlist { get; set; }
}
