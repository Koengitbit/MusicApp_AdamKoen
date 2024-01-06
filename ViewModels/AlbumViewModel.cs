namespace MusicApp_AdamKoen.ViewModels;

public class AlbumViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime ReleaseYear { get; set; }
    public List<SongViewModel> Songs { get; set; }
    public bool IsLiked { get; set; }
    public int TotalSongs { get; set; }
    public int TotalDuration { get; set; }

}
