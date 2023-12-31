namespace MusicApp_AdamKoen.Models;

public class Album
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public DateTime Release_Year { get; set; }
    public List<AlbumSong>? Songs { get; set; }
}