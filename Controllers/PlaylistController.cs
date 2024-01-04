using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                        Id = ps.Song.Id,
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
        public async Task<IActionResult> Details(int id)
        {
            var playlist = await _db.Playlists
                .Where(p => p.Id == id)
                .Select(p => new PlaylistViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    IsPublic = p.IsPublic,
                    CreatedAt = p.CreatedAt,
                    UserId = p.UserId,
                    Songs = p.Songs.Select(ps => new SongViewModel
                    {
                        Id = ps.Song.Id,
                        Title = ps.Song.Title,
                        ArtistName = ps.Song.Artist.Name,
                        Genre = ps.Song.Genre,
                        ReleaseDate = ps.Song.ReleaseDate,
                        Duration = ps.Song.Duration,
                        Playlists = ps.Song.Playlists.Select(pl => pl.Playlist.Name).ToList()
                    }).ToList()
                }).FirstOrDefaultAsync();
           
            if (playlist == null)
            {
                return NotFound();
            }
            ViewBag.Songs = new SelectList(await _db.Songs.ToListAsync(), "Id", "Title");
            return View(playlist);
        }
        public async Task<IActionResult> MyPlaylists()
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
                    Id = ps.Song.Id,
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
                MyPlaylists = myPlaylists
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddSongToPlaylist(int playlistId, int? selectedSongId)
        {
            if (selectedSongId.HasValue)
            {
                var playlist = await _db.Playlists.FindAsync(playlistId);

                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Check if the playlist belongs to the current user
                if (playlist != null && playlist.UserId == currentUserId)
                {
                    // Check if the song already exists in the playlist
                    var existingSong = await _db.PlaylistSongs
                        .FirstOrDefaultAsync(ps => ps.PlaylistId == playlistId && ps.SongId == selectedSongId.Value);

                    if (existingSong == null)
                    {
                        var playlistSong = new PlaylistSong { PlaylistId = playlistId, SongId = selectedSongId.Value };
                        _db.PlaylistSongs.Add(playlistSong);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "This song is already in the playlist.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "You can only add songs to your own playlists.";
                }
            }
            return RedirectToAction("Details", new { id = playlistId });
        }
        [HttpPost]
        public async Task<IActionResult> RemoveSongFromPlaylist(int playlistId, int songToRemoveId)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var playlist = await _db.Playlists.FindAsync(playlistId);

            if (playlist != null && playlist.UserId == currentUserId)
            {
                var song = await _db.PlaylistSongs
                    .FirstOrDefaultAsync(ps => ps.PlaylistId == playlistId && ps.SongId == songToRemoveId);

                if (song != null)
                {
                    _db.PlaylistSongs.Remove(song);
                    await _db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Details", new { id = playlistId });
        }


    }
}
