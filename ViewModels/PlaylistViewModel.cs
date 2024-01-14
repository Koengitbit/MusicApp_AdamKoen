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
        public bool IsLiked { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public List<SongViewModel>? Songs { get; set; }
        public int? SelectedSongId { get; set; }
        public int TotalSongs { get; set; }
        public int TotalDuration { get; set; }
        public bool IsEnriched { get; set; }
        public List<PlaylistViewModel>? OtherPlaylists { get; set; }
    }
}
