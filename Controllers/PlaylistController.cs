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
                        Playlists = ps.Song.Playlists.Select(pl => pl.Playlist.Name).ToList(),
                        OrderIndex = ps.OrderIndex,
                    }).OrderBy(s => s.OrderIndex).ToList(),
                    TotalSongs = p.Songs.Count,
                    TotalDuration = p.Songs.Sum(ps => ps.Song.Duration)
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
        [Authorize]
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
                        ArtistId = ps.Song.ArtistId,
                        ArtistName = ps.Song.Artist.Name,
                        Genre = ps.Song.Genre,
                        ReleaseDate = ps.Song.ReleaseDate,
                        Duration = ps.Song.Duration,
                        Playlists = ps.Song.Playlists.Select(pl => pl.Playlist.Name).ToList(),
                        OrderIndex = ps.OrderIndex
                    }).OrderBy(s => s.OrderIndex).ToList(),
                    TotalSongs = p.Songs.Count,
                    TotalDuration = p.Songs.Sum(ps => ps.Song.Duration)
                }).FirstOrDefaultAsync();
           
            if (playlist == null)
            {
                return NotFound();
            }
            var otherPlaylists = await _db.Playlists
                                  .Where(p => p.Id != id)
                                  .Select(p => new PlaylistViewModel { Id = p.Id, Name = p.Name })
                                  .ToListAsync();

            playlist.OtherPlaylists = otherPlaylists;
            ViewBag.Songs = new SelectList(await _db.Songs.ToListAsync(), "Id", "Title");

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var hasLikedPlaylist = await _db.Likes.AnyAsync(l => l.UserId == currentUserId && l.PlaylistId == id);
            playlist.IsLiked = hasLikedPlaylist;

            
            var likedSongIds = await _db.Likes
                                .Where(l => l.UserId == currentUserId && l.SongId != null)
                                .Select(l => l.SongId.Value)
                                .ToListAsync();

            playlist.Songs.ForEach(s => s.IsLiked = likedSongIds.Contains(s.Id));
            return View(playlist);
        }
        [Authorize]
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
                    Playlists = ps.Song.Playlists.Select(pl => pl.Playlist.Name).ToList(),
                    OrderIndex = ps.OrderIndex
                }).OrderBy(s => s.OrderIndex).ToList()
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
                        var maxOrderIndex = await _db.PlaylistSongs
                                         .Where(ps => ps.PlaylistId == playlistId)
                                         .MaxAsync(ps => (int?)ps.OrderIndex) ?? -1;

                        var playlistSong = new PlaylistSong { PlaylistId = playlistId, SongId = selectedSongId.Value, OrderIndex = maxOrderIndex + 1 };
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
            else
            {
                TempData["ErrorMessage"] = "You can only remove songs from your own playlists.";
            }
            return RedirectToAction("Details", new { id = playlistId });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSongOrder(int playlistId, [FromBody] List<int> songIds)
        {
            var playlistSongs = await _db.PlaylistSongs
                                         .Where(ps => ps.PlaylistId == playlistId)
                                         .ToListAsync();

            for (int i = 0; i < songIds.Count; i++)
            {
                var songId = songIds[i];
                var playlistSong = playlistSongs.FirstOrDefault(ps => ps.SongId == songId);
                if (playlistSong != null)
                {
                    playlistSong.OrderIndex = i;
                }
            }

            await _db.SaveChangesAsync();
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> AddPlaylistToPlaylist(int targetPlaylistId, int sourcePlaylistId)
        {
            var targetPlaylist = await _db.Playlists.Include(p => p.Songs).FirstOrDefaultAsync(p => p.Id == targetPlaylistId);
            var sourcePlaylist = await _db.Playlists.Include(p => p.Songs).FirstOrDefaultAsync(p => p.Id == sourcePlaylistId);

            if (targetPlaylist == null || sourcePlaylist == null)
            {
                TempData["ErrorMessage"] = "One or both playlists not found.";
                return RedirectToAction(nameof(Details), new { id = targetPlaylistId });
            }

            foreach (var song in sourcePlaylist.Songs)
            {
                if (targetPlaylist.Songs.All(ps => ps.SongId != song.SongId))
                {
                    targetPlaylist.Songs.Add(new PlaylistSong
                    {
                        SongId = song.SongId,
                        PlaylistId = targetPlaylistId,
                        OrderIndex = targetPlaylist.Songs.Max(ps => (int?)ps.OrderIndex) ?? -1 + 1
                    });
                }
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = targetPlaylistId });
        }
        
        [HttpPost]
        public async Task<IActionResult> Like([FromBody] LikeRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (string.IsNullOrEmpty(request.Type))
            {
                return Json(new { success = false, message = "Type is null or empty." });
            }
            // Determine the type and add the like accordingly
            switch (request.Type.ToLower())
            {
                case "song":
                    var songLike = await _db.Likes.FirstOrDefaultAsync(l => l.SongId == request.ItemId && l.UserId == userId);
                    if (songLike == null)
                    {
                        _db.Likes.Add(new Like { UserId = userId, SongId = request.ItemId });
                        await _db.SaveChangesAsync();
                        return Json(new { success = true, liked = true, message = "Song liked successfully." });
                    }
                    else
                    {
                        _db.Likes.Remove(songLike);
                        await _db.SaveChangesAsync();
                        return Json(new { success = true, liked = false, message = "Song unliked successfully." });
                    }
                case "album":
                    var albumLike = await _db.Likes.FirstOrDefaultAsync(l => l.AlbumId == request.ItemId && l.UserId == userId);
                    if (albumLike == null)
                    {
                        _db.Likes.Add(new Like { UserId = userId, AlbumId = request.ItemId });
                        await _db.SaveChangesAsync();
                        return Json(new { success = true, liked = true, message = "Album liked successfully." });
                    }
                    else
                    {
                        _db.Likes.Remove(albumLike);
                        await _db.SaveChangesAsync();
                        return Json(new { success = true, liked = false, message = "Album unliked successfully." });
                    }

                case "artist":
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

                case "playlist":
                    var playlistLike = await _db.Likes.FirstOrDefaultAsync(l => l.PlaylistId == request.ItemId && l.UserId == userId);
                    if (playlistLike == null)
                    {
                        _db.Likes.Add(new Like { UserId = userId, PlaylistId = request.ItemId });
                        await _db.SaveChangesAsync();
                        return Json(new { success = true, liked = true, message = "Playlist liked successfully." });
                    }
                    else
                    {
                        _db.Likes.Remove(playlistLike);
                        await _db.SaveChangesAsync();
                        return Json(new { success = true, liked = false, message = "Playlist unliked successfully." });
                    }

                default:
                    return Json(new { success = false, message = "Invalid like type." });
            }
        }




    }
}
