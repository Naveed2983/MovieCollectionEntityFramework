using MovieCollection.Core.Interfaces;
using MovieCollection.Core.Models;
using MovieCollection.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Services.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository movieRepository;
        private readonly IGenreRepository genreRepository;

        public MovieService(IMovieRepository movieRepository, IGenreRepository genreRepository)
        {
            this.movieRepository = movieRepository;
            this.genreRepository = genreRepository;
        }

        public async Task AddMovieAsync(Movie movie)
        {
            if (string.IsNullOrWhiteSpace(movie.Title))
                throw new Exception("Movie title is required.");

            if (movie.Duration <= 0)
                throw new Exception("Duration must be positive.");

            if (movie.ReleaseYear < 1900 || movie.ReleaseYear > DateTime.Now.Year + 1)
                throw new Exception("Release year is invalid.");

            if (movie.Rating == null)
                throw new Exception("Rating is required.");

            if (movie.Rating.ImdbRating < 0 || movie.Rating.ImdbRating > 10)
                throw new Exception("IMDb rating must be between 0 and 10.");

            if (movie.Rating.Votes < 0)
                throw new Exception("Votes cannot be negative.");

            var validGenres = await genreRepository.GetAllGenresAsync();
            var validIds = validGenres.Select(g => g.GenreId).ToList();
            foreach (var genre in movie.Genres)
            {
                if (!validIds.Contains(genre.GenreId))
                    throw new Exception($"Invalid genre ID: {genre.GenreId}");
            }
            await movieRepository.AddMovieAsync(movie);
        }
        public async Task DeleteMovieAsync(int movieId, int userId)
        {
            var movie = await GetMovieByIdAsync(movieId, userId);

            if (movie == null)
                throw new Exception("Invalid Movie ID.");

            await movieRepository.DeleteMovieAsync(movieId, userId);
        }
        public async Task<List<Movie>> GetAllMoviesByUserIdAsync(int userId)
        {
            var movies = await movieRepository.GetAllMoviesByUserIdAsync(userId);

            if (movies == null || movies.Count == 0)
                throw new InvalidOperationException("No movies found.");

            return movies;
        }
        public async Task<Movie?> GetMovieByIdAsync(int movieId, int userId)
        {
            return await movieRepository.GetMovieByIdAsync(movieId, userId);
        }
        public async Task<List<Movie>> SearchMoviesByUserIdAsync(int userId, string searchTerm)
        {
            var movies = await movieRepository.SearchMoviesByUserIdAsync(userId,searchTerm);

            if (movies == null || movies.Count == 0)
                throw new InvalidOperationException("No movies found.");

            return movies;
        }
        public async Task UpdateMovieAsync(Movie updatedMovie)
        {
            var existingMovie = await movieRepository.GetMovieByIdAsync(updatedMovie.MovieId, updatedMovie.UserId);
            if (existingMovie == null)
                throw new Exception("Movie not found.");

            if (!string.IsNullOrWhiteSpace(updatedMovie.Title))
                existingMovie.Title = updatedMovie.Title;

            if (updatedMovie.Duration > 0)
                existingMovie.Duration = updatedMovie.Duration;

            if (updatedMovie.ReleaseYear >= 1800 && updatedMovie.ReleaseYear <= DateTime.Now.Year)
                existingMovie.ReleaseYear = updatedMovie.ReleaseYear;

            if (updatedMovie.Genres != null && updatedMovie.Genres.Count > 0)
            {
                var genreIds = updatedMovie.Genres.Select(g => g.GenreId).ToList();
                var trackedGenres = await genreRepository.GetGenresByIdsAsync(genreIds);

                existingMovie.Genres.Clear();
                foreach (var genre in trackedGenres)
                {
                    existingMovie.Genres.Add(genre);
                }
            }

            if (updatedMovie.Rating != null)
            {
                if (existingMovie.Rating == null)
                    existingMovie.Rating = updatedMovie.Rating;
                else
                {
                    existingMovie.Rating.ImdbRating = updatedMovie.Rating.ImdbRating;
                    existingMovie.Rating.Votes = updatedMovie.Rating.Votes;
                    existingMovie.Rating.AgeRating = updatedMovie.Rating.AgeRating;
                }
            }

            await movieRepository.UpdateMovieAsync(existingMovie);
        }
    }
}
