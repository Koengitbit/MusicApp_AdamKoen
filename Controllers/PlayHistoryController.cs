using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApp_AdamKoen.DAL;
using MusicApp_AdamKoen.Models;
using MusicApp_AdamKoen.ViewModels;
using MusicApp_AdamKoen.Controllers;
namespace MusicApp_AdamKoen.Controllers
{
    [Route("PlayHistory")]
    public class PlayHistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly SpotifyDbContext _db;


        public PlayHistoryController(SpotifyDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("recordPlay")]
        public async Task<IActionResult> TrackPlay([FromBody] PlayHistoryViewModel playHistoryViewModel)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var playHistory = new PlayHistory
            {
                UserId = userId,
                SongId = playHistoryViewModel.SongId,
                PlayedAt = DateTime.UtcNow
            };

            _db.PlayHistory.Add(playHistory);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Track play recorded successfully." });
        }




        private async Task CreateOrUpdatePlaylist(int userId)
        {
            // Fetch liked songs
            var likedSongIds = await _db.Likes
                                        .Where(l => l.UserId == userId && l.SongId != null)
                                        .Select(l => l.SongId.Value)
                                        .Distinct() // Ensure uniqueness
                                        .ToListAsync();

            // Fetch songs from play history
            var playedSongIds = await _db.PlayHistory
                                         .Where(ph => ph.UserId == userId)
                                         .OrderByDescending(ph => ph.PlayedAt)
                                         .Select(ph => ph.SongId)
                                         .Distinct() // Ensure uniqueness
                                         .ToListAsync();

            // Combine liked songs and played songs
            var combinedSongIds = likedSongIds.Union(playedSongIds).ToList();

            // Fetch the latest playlist or create a new one
            var playlist = await _db.Playlists
                                    .OrderByDescending(p => p.CreatedAt)
                                    .FirstOrDefaultAsync(p => p.UserId == userId && p.Name == "My Favorites and History");

            if (playlist == null)
            {
                playlist = new Playlist
                {
                    Name = "My Favorites and History",
                    IsPublic = true, 
                    CreatedAt = DateTime.Now,
                    UserId = userId,
                    Songs = new List<PlaylistSong>()
                };
                _db.Playlists.Add(playlist);
            }
            else
            {
                playlist.CreatedAt = DateTime.Now; 
                playlist.Songs.Clear(); // Clear existing songs
            }

            // Add songs to the playlist
            foreach (var songId in combinedSongIds)
            {
                playlist.Songs.Add(new PlaylistSong { SongId = songId });
            }

            await _db.SaveChangesAsync();
        }

    }

}
