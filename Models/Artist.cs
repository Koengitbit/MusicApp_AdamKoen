namespace MusicApp_AdamKoen.Models;

public class Artist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Song> Songs { get; set; }
}
