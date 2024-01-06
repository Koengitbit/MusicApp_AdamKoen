namespace MusicApp_AdamKoen.ViewModels
{
    public class ArtistViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SongViewModel> Songs { get; set; }
        public int TotalSongs { get; set; }
        public int TotalDuration { get; set; }
        public bool IsLiked { get; set; }
    }
}
