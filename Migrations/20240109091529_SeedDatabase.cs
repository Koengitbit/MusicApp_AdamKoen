using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MusicAppAdamKoen.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AlbumSongs",
                columns: new[] { "AlbumId", "SongId", "OrderIndex" },
                values: new object[] { 2, 2, 10 });

            migrationBuilder.UpdateData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 1,
                column: "Release_Year",
                value: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 2,
                column: "Release_Year",
                value: new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Playlist 1");

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Playlist 2");

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "ArtistId", "Duration", "Genre", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { 10, 2, 278, "Hip Hop", new DateTime(1996, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ambitionz Az A Ridah" },
                    { 11, 2, 276, "Hip Hop", new DateTime(1996, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "All About U" },
                    { 12, 2, 248, "Hip Hop", new DateTime(1996, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Skandalouz" },
                    { 13, 2, 312, "Hip Hop", new DateTime(1996, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Got My Mind Made Up" },
                    { 14, 2, 285, "Hip Hop", new DateTime(1996, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "How Do U Want It" },
                    { 15, 2, 246, "Hip Hop", new DateTime(1996, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2 Of Amerikaz Most Wanted" },
                    { 16, 2, 374, "Hip Hop", new DateTime(1996, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "No More Pain" },
                    { 17, 2, 283, "Hip Hop", new DateTime(1996, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Life Goes On" },
                    { 18, 2, 314, "Hip Hop", new DateTime(1996, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Only God Can Judge Me" },
                    { 19, 2, 314, "Hip Hop", new DateTime(1996, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tradin' War Stories" }
                });

            migrationBuilder.InsertData(
                table: "AlbumSongs",
                columns: new[] { "AlbumId", "SongId", "OrderIndex" },
                values: new object[,]
                {
                    { 2, 10, 0 },
                    { 2, 11, 1 },
                    { 2, 12, 2 },
                    { 2, 13, 3 },
                    { 2, 14, 4 },
                    { 2, 15, 5 },
                    { 2, 16, 6 },
                    { 2, 17, 7 },
                    { 2, 18, 8 },
                    { 2, 19, 9 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 2, 10 });

            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 2, 11 });

            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 2, 12 });

            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 2, 13 });

            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 2, 14 });

            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 2, 15 });

            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 2, 16 });

            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 2, 17 });

            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 2, 18 });

            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 2, 19 });

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.UpdateData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 1,
                column: "Release_Year",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Albums",
                keyColumn: "Id",
                keyValue: 2,
                column: "Release_Year",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Playlist One");

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Playlist Two");
        }
    }
}
