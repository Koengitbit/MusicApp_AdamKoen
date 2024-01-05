using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MusicAppAdamKoen.Migrations
{
    /// <inheritdoc />
    public partial class AddLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "AlbumSongs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SongId = table.Column<int>(type: "int", nullable: true),
                    AlbumId = table.Column<int>(type: "int", nullable: true),
                    ArtistId = table.Column<int>(type: "int", nullable: true),
                    PlaylistId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Likes_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Likes_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Likes_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Likes_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Likes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AlbumSongs",
                columns: new[] { "AlbumId", "SongId", "OrderIndex" },
                values: new object[,]
                {
                    { 1, 1, 0 },
                    { 1, 3, 1 },
                    { 1, 4, 2 },
                    { 1, 5, 3 },
                    { 1, 6, 4 },
                    { 1, 7, 5 },
                    { 1, 8, 6 },
                    { 1, 9, 7 }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Likes_AlbumId",
                table: "Likes",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_ArtistId",
                table: "Likes",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_PlaylistId",
                table: "Likes",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_SongId",
                table: "Likes",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId",
                table: "Likes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 1, 6 });

            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 1, 7 });

            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 1, 8 });

            migrationBuilder.DeleteData(
                table: "AlbumSongs",
                keyColumns: new[] { "AlbumId", "SongId" },
                keyValues: new object[] { 1, 9 });

            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "AlbumSongs");

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
        }
    }
}
