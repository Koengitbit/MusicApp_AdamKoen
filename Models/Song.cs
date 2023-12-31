using System.ComponentModel.DataAnnotations;

namespace MusicApp_AdamKoen.Models;

public class Song
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }

    [Display(Name = "Release Date")]
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }
    public int Duration { get; set; }
    public int ArtistId { get; set; }
    public Artist Artist { get; set; }
    public List<PlaylistSong> Playlists { get; set; }
    public List<AlbumSong> Albums { get; set; }
}