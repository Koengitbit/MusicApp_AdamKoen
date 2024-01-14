namespace MusicApp_AdamKoen.ViewModels;

public class SongViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int ArtistId { get; set; }
    public string ArtistName { get; set; }
    public string Genre { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int Duration { get; set; }
    public bool IsLiked { get; set; }
    public List<string> Playlists { get; set; }
    public bool IsEnriched { get; set; }
    public int OrderIndex { get; set; }
}
