using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApp_AdamKoen.DAL;
using MusicApp_AdamKoen.Models;
using MusicApp_AdamKoen.ViewModels;

namespace MusicApp_AdamKoen.Controllers;

public class OverviewController : Controller
{
    private readonly SpotifyDbContext _db;

    public OverviewController(SpotifyDbContext db)
    {
        _db = db;
    }

    private async Task<List<int>> GetLikedSongIds(int userId)
    {
        return await _db.Likes
                        .Where(l => l.UserId == userId && l.SongId != null)
                        .Select(l => l.SongId.Value)
                        .ToListAsync();
    }
    private int GetCurrentUserId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    }

    [Authorize]
    public async Task<IActionResult> Index(string searchString)
    {
        ViewData["searchString"] = searchString;
        //Get logged in userId
        var currentUserId = GetCurrentUserId();
        //Get all liked songIds
        var likedSongIds = await GetLikedSongIds(currentUserId);

        IQueryable<Song> songQuery = _db.Songs
        .Include(s => s.Artist)
        .Include(s => s.Playlists)
            .ThenInclude(ps => ps.Playlist);

        if (!String.IsNullOrEmpty(searchString))
        {
            songQuery = songQuery.Where(s => s.Title.Contains(searchString));
        }

        var songs = await songQuery
            .Select(s => new SongViewModel
            {
                Id = s.Id,
                Title = s.Title,
                ArtistId = s.Artist.Id,
                ArtistName = s.Artist.Name,
                Genre = s.Genre,
                ReleaseDate = s.ReleaseDate,
                Duration = s.Duration,
                Playlists = s.Playlists.Select(p => p.Playlist.Name).ToList(),
                IsLiked = likedSongIds.Contains(s.Id)
            }).ToListAsync();
        return View(songs);
    }

    [Authorize]
    public async Task<IActionResult> MusicLibrary()
    {
        var currentUserId = GetCurrentUserId();
        var likedSongIds = await GetLikedSongIds(currentUserId);

        // Get only liked songs
        var likedSongs = await _db.Songs
            .Where(s => likedSongIds.Contains(s.Id))
            .Include(s => s.Artist)
            .Include(s => s.Playlists)
                .ThenInclude(ps => ps.Playlist)
            .Select(s => new SongViewModel
            {
                Id = s.Id,
                Title = s.Title,
                ArtistId = s.Artist.Id,
                ArtistName = s.Artist.Name,
                Genre = s.Genre,
                ReleaseDate = s.ReleaseDate,
                Duration = s.Duration,
                Playlists = s.Playlists.Select(p => p.Playlist.Name).ToList(),
                IsLiked = likedSongIds.Contains(s.Id)
            })
            .ToListAsync();

        return View(likedSongs);
    }

}


