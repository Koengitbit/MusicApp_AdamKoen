using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApp_AdamKoen.DAL;
using MusicApp_AdamKoen.Models;
using MusicApp_AdamKoen.ViewModels;

namespace MusicApp_AdamKoen.Controllers;

public class AlbumController : Controller
{
    private readonly SpotifyDbContext _db;

    public AlbumController(SpotifyDbContext db)
    {
        _db = db;
    }
    [Authorize]
    public IActionResult Index()
    {
        var albums = _db.Albums.Select(a => new AlbumViewModel
        {
            Id = a.Id,
            Title = a.Title,
            ReleaseYear = a.Release_Year,
            Songs = a.Songs.Select(s => new SongViewModel
            {
                Id = s.SongId,
                Title = s.Song.Title,
                ArtistId = s.Song.ArtistId,
                ArtistName = s.Song.Artist.Name,
                Genre = s.Song.Genre,
                ReleaseDate = s.Song.ReleaseDate,
                Duration = s.Song.Duration,
            }).ToList()
        }).ToList();

        return View(albums);
    }

    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var album = await _db.Albums
            .Where(a => a.Id == id)
            .Select(a => new AlbumViewModel
            {
                Id = a.Id,
                Title = a.Title,
                ReleaseYear = a.Release_Year,
                Songs = a.Songs.Select(s => new SongViewModel
                {
                    Id = s.Song.Id,
                    Title = s.Song.Title,
                    ArtistId = s.Song.Artist.Id,
                    ArtistName = s.Song.Artist.Name,
                    Genre = s.Song.Genre,
                    ReleaseDate = s.Song.ReleaseDate,
                    Duration = s.Song.Duration
                }).ToList(),
                TotalSongs = a.Songs.Count,
                TotalDuration = a.Songs.Sum(s => s.Song.Duration)
            })
            .FirstOrDefaultAsync();

        if (album == null)
        {
            return NotFound();
        }
        var hasLikedAlbum = await _db.Likes.AnyAsync(l => l.UserId == currentUserId && l.AlbumId == id);
        album.IsLiked = hasLikedAlbum;

        var likedSongIds = await _db.Likes
                            .Where(l => l.UserId == currentUserId && l.SongId != null)
                            .Select(l => l.SongId.Value)
                            .ToListAsync();

        album.Songs.ForEach(s => s.IsLiked = likedSongIds.Contains(s.Id));

        return View(album);
    }



    [HttpPost]
    public async Task<IActionResult> Like([FromBody] LikeRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        if (string.IsNullOrEmpty(request.Type) || request.ItemId <= 0)
        {
            return Json(new { success = false, message = "Invalid request." });
        }

        switch (request.Type.ToLower())
        {
            case "album":
                var albumLike = await _db.Likes.FirstOrDefaultAsync(l => l.AlbumId == request.ItemId && l.UserId == userId);
                if (albumLike == null)
                {
                    _db.Likes.Add(new Like { UserId = userId, AlbumId = request.ItemId });
                    await _db.SaveChangesAsync();
                    return Json(new { success = true, liked = true });
                }
                else
                {
                    _db.Likes.Remove(albumLike);
                    await _db.SaveChangesAsync();
                    return Json(new { success = true, liked = false });
                }

            default:
                return Json(new { success = false, message = "Invalid like type." });
        }
    }
}
