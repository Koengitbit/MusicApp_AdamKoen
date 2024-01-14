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
    }
}
