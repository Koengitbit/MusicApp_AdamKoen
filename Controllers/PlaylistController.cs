using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MusicApp_AdamKoen.DAL;
using MusicApp_AdamKoen.Models;
using MusicApp_AdamKoen.ViewModels;


namespace MusicApp_AdamKoen.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly SpotifyDbContext _db;

        public PlaylistController(SpotifyDbContext db)
        {
            _db = db;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var myPlaylists = await _db.Playlists
            .Where(p => p.UserId == currentUserId)
            .Select(p => new PlaylistViewModel
            {
                Id = p.Id,
                Name = p.Name,
                IsPublic = p.IsPublic,
                CreatedAt = p.CreatedAt,
                UserId = p.UserId,
                Songs = p.Songs.Select(ps => new SongViewModel
                {
                    Title = ps.Song.Title,
                    ArtistName = ps.Song.Artist.Name,
                    Genre = ps.Song.Genre,
                    ReleaseDate = ps.Song.ReleaseDate,
                    Duration = ps.Song.Duration,
                    Playlists = ps.Song.Playlists.Select(pl => pl.Playlist.Name).ToList()
                }).ToList()
            }).ToListAsync();
            var allPlaylists = await _db.Playlists
                .Select(p => new PlaylistViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    IsPublic = p.IsPublic,
                    CreatedAt = p.CreatedAt,
                    UserId = p.UserId,
                    Songs = p.Songs.Select(ps => new SongViewModel
                    {
                        Title = ps.Song.Title,
                        ArtistName = ps.Song.Artist.Name,
                        Genre = ps.Song.Genre,
                        ReleaseDate = ps.Song.ReleaseDate,
                        Duration = ps.Song.Duration,
                        Playlists = ps.Song.Playlists.Select(pl => pl.Playlist.Name).ToList()
                    }).ToList()
                }).ToListAsync();

            var viewModel = new IndexViewModel
            {
                MyPlaylists = myPlaylists,
                AllPlaylists = allPlaylists
            };
            return View(viewModel);
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsPublic")] PlaylistViewModel playlistViewModel)
        {

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var newPlaylist = new Playlist
                {
                    Name = playlistViewModel.Name,
                    IsPublic = playlistViewModel.IsPublic,
                    CreatedAt = DateTime.Now,
                    UserId = int.Parse(userId),
                    Songs = new List<PlaylistSong>()
                };

                _db.Playlists.Add(newPlaylist);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(playlistViewModel);
        }

    }
}
