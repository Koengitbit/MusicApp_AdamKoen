namespace MusicApp_AdamKoen.Models;

public class PlaylistSong
{
    public int PlaylistId { get; set; }
    public Playlist Playlist { get; set; }
    public int SongId { get; set; }
    public Song Song { get; set; }
    public int OrderIndex { get; set; }
    public bool IsEnriched { get; set; }
}