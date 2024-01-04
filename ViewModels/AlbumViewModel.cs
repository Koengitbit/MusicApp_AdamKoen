namespace MusicApp_AdamKoen.ViewModels;

public class AlbumViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime ReleaseYear { get; set; }
    public List<SongViewModel> Songs { get; set; }
}
