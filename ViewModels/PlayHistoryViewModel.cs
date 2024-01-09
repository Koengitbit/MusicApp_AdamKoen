namespace MusicApp_AdamKoen.ViewModels
{
    public class PlayHistoryViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SongId { get; set; }
        public DateTime PlayedAt { get; set; }
    }
}
