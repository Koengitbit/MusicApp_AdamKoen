namespace MusicApp_AdamKoen.ViewModels;

public class SongViewModel
{
    public string Title { get; set; }
    public string ArtistName { get; set; }
    public string Genre { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int Duration { get; set; }
    public List<string> Playlists { get; set; }
}
