using System.ComponentModel.DataAnnotations;

namespace MusicApp_AdamKoen.Models;


public class Playlist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsPublic { get; set; }
    [Display(Name = "Creation At")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public List<PlaylistSong>? Songs { get; set; }

}