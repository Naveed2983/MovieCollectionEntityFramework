using Microsoft.EntityFrameworkCore;
using MovieCollection.Core.Interfaces;
using MovieCollection.Core.Models;
using MovieCollection.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.DAL.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieContext _context;
        public MovieRepository(MovieContext context)
        {
            _context = context;
        }
        public async Task AddMovieAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteMovieAsync(int movieId, int userId)
        {
            var movie = await _context.Movies
                .Include(m => m.Rating)
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.MovieId == movieId && m.UserId == userId);

            if (movie != null)
            {
                movie.Genres.Clear();

                if (movie.Rating != null)
                    _context.MovieRatings.Remove(movie.Rating);

                _context.Movies.Remove(movie);

                await _context.SaveChangesAsync();
            }
        }
        public async Task<Movie?> GetMovieByIdAsync(int movieId, int userId)
        {
            return await _context.Movies
                .Include(m => m.Rating)
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.MovieId == movieId && m.UserId == userId);
        }
        public async Task<List<Movie>> GetAllMoviesByUserIdAsync(int userId)
        {
            return await _context.Movies
                .Where(m => m.UserId == userId)
                .Include(m => m.Rating)
                .Include(m => m.Genres)
                .ToListAsync();
        }
        public async Task<List<Movie>> SearchMoviesByUserIdAsync(int userId, string searchTerm)
        {
            return await _context.Movies
                .Include (m => m.Rating)
                .Include(m => m.Genres)
                .Where(m=>m.Title.Contains(searchTerm)&&m.UserId==userId)
                .ToListAsync() ;
        }
        public async Task UpdateMovieAsync(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }
    }
}
