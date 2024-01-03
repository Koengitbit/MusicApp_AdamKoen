using System.ComponentModel.DataAnnotations;

namespace MusicApp_AdamKoen.ViewModels
{
    public class PlaylistViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public List<SongViewModel>? Songs { get; set; }
    }
}
