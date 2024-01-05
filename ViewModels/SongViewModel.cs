namespace MusicApp_AdamKoen.ViewModels;

public class SongViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ArtistName { get; set; }
    public string Genre { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int Duration { get; set; }
    public List<string> Playlists { get; set; }
    //Is playing is still not in use, might have to delete soon I think.
    public bool IsPlaying { get; set; }
    public int OrderIndex { get; set; }
}
