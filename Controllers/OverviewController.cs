using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApp_AdamKoen.DAL;
using MusicApp_AdamKoen.ViewModels;

namespace MusicApp_AdamKoen.Controllers;

public class OverviewController : Controller
{
    private readonly SpotifyDbContext _db;

    public OverviewController(SpotifyDbContext db)
    {
        _db = db;
    }
    [Authorize]
    public async Task<IActionResult> Index()
    {
        //Get logged in userId
        var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        //Get all liked songIds
        var likedSongIdAll = await _db.Likes
                                .Where(l => l.UserId == currentUserId && l.SongId != null)
                                .Select(l => l.SongId.Value)
                                .ToListAsync();

        var songs = _db.Songs
            .Include(s => s.Artist)
            .Include(s => s.Playlists)
                .ThenInclude(ps => ps.Playlist)
            .Select(s => new SongViewModel
            {
                Id = s.Id,
                Title = s.Title,
                ArtistName = s.Artist.Name,
                Genre = s.Genre,
                ReleaseDate = s.ReleaseDate,
                Duration = s.Duration,
                Playlists = s.Playlists.Select(p => p.Playlist.Name).ToList(),
                IsLiked = likedSongIdAll.Contains(s.Id)
            }).ToList();
        return View(songs);
    }
    
}


