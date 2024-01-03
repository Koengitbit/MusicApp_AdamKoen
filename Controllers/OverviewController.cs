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
    public IActionResult Index()
    {
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
                Playlists = s.Playlists.Select(p => p.Playlist.Name).ToList()
            })
            .ToList();
        return View(songs);
    }
}
