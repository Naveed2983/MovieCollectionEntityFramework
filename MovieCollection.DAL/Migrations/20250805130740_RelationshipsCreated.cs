using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieCollection.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RelationshipsCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GenreMovie_Genres_GenresGenreId",
                table: "GenreMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_GenreMovie_Movies_MoviesMovieId",
                table: "GenreMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GenreMovie",
                table: "GenreMovie");

            migrationBuilder.RenameTable(
                name: "GenreMovie",
                newName: "MovieGenres");

            migrationBuilder.RenameIndex(
                name: "IX_GenreMovie_MoviesMovieId",
                table: "MovieGenres",
                newName: "IX_MovieGenres_MoviesMovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieGenres",
                table: "MovieGenres",
                columns: new[] { "GenresGenreId", "MoviesMovieId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Genres_GenresGenreId",
                table: "MovieGenres",
                column: "GenresGenreId",
                principalTable: "Genres",
                principalColumn: "GenreId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Movies_MoviesMovieId",
                table: "MovieGenres",
                column: "MoviesMovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Genres_GenresGenreId",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Movies_MoviesMovieId",
                table: "MovieGenres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieGenres",
                table: "MovieGenres");

            migrationBuilder.RenameTable(
                name: "MovieGenres",
                newName: "GenreMovie");

            migrationBuilder.RenameIndex(
                name: "IX_MovieGenres_MoviesMovieId",
                table: "GenreMovie",
                newName: "IX_GenreMovie_MoviesMovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GenreMovie",
                table: "GenreMovie",
                columns: new[] { "GenresGenreId", "MoviesMovieId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GenreMovie_Genres_GenresGenreId",
                table: "GenreMovie",
                column: "GenresGenreId",
                principalTable: "Genres",
                principalColumn: "GenreId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GenreMovie_Movies_MoviesMovieId",
                table: "GenreMovie",
                column: "MoviesMovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
