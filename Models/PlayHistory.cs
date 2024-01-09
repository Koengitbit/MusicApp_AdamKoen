namespace MusicApp_AdamKoen.Models
{
    public class PlayHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SongId { get; set; }
        public DateTime PlayedAt { get; set; }
    }
}
