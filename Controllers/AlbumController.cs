using Microsoft.AspNetCore.Mvc;
using MusicApp_AdamKoen.DAL;
using MusicApp_AdamKoen.ViewModels;

namespace MusicApp_AdamKoen.Controllers;

public class AlbumController : Controller
{
    private readonly SpotifyDbContext _db;

    public AlbumController(SpotifyDbContext db)
    {
        _db = db;
    }

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
                ArtistName = s.Song.Artist.Name,
                Genre = s.Song.Genre,
                ReleaseDate = s.Song.ReleaseDate,
                Duration = s.Song.Duration,
            }).ToList()
        }).ToList();

        return View(albums);
    }
}
