using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApp_AdamKoen.DAL; // Adjust namespace as per your project structure
using MusicApp_AdamKoen.ViewModels;

namespace MusicApp_AdamKoen.Controllers
{
    public class ArtistController : Controller
    {
        private readonly SpotifyDbContext _db;

        public ArtistController(SpotifyDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Details(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var artist = await _db.Artists
                .Where(a => a.Id == id)
                .Select(a => new ArtistViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Songs = a.Songs.Select(s => new SongViewModel
                    {
                        Id = s.Id,
                        Title = s.Title,
                        ArtistId = s.ArtistId,
                        ArtistName = s.Artist.Name,
                        Genre = s.Genre,
                        ReleaseDate = s.ReleaseDate,
                        Duration = s.Duration,
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (artist == null)
            {
                return NotFound();
            }
            var hasLikedArtist = await _db.Likes.AnyAsync(l => l.UserId == currentUserId && l.ArtistId == id);
            artist.IsLiked = hasLikedArtist;

            artist.TotalSongs = artist.Songs.Count;
            artist.TotalDuration = artist.Songs.Sum(s => s.Duration);
            return View(artist);
        }
        [HttpPost]
        public async Task<IActionResult> Like([FromBody] LikeRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (string.IsNullOrEmpty(request.Type) || request.ItemId <= 0)
            {
                return Json(new { success = false, message = "Invalid request." });
            }

            if (request.Type.ToLower() == "artist")
            {
                var artistLike = await _db.Likes.FirstOrDefaultAsync(l => l.ArtistId == request.ItemId && l.UserId == userId);
                if (artistLike == null)
                {
                    _db.Likes.Add(new Like { UserId = userId, ArtistId = request.ItemId });
                    await _db.SaveChangesAsync();
                    return Json(new { success = true, liked = true, message = "Artist liked successfully." });
                }
                else
                {
                    _db.Likes.Remove(artistLike);
                    await _db.SaveChangesAsync();
                    return Json(new { success = true, liked = false, message = "Artist unliked successfully." });
                }
            }

            return Json(new { success = false, message = "Invalid like type." });
        }

    }
}
