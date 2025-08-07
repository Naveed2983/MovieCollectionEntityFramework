using MovieCollection.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieCollection.Core.Interfaces
{
    public interface IMovieService
    {
        Task AddMovieAsync(Movie movie);
        Task<List<Movie>> GetAllMoviesByUserIdAsync(int userId);
        Task<Movie?> GetMovieByIdAsync(int movieId, int userId);
        Task UpdateMovieAsync(Movie movie);
        Task DeleteMovieAsync(int movieId, int userId);
        Task<List<Movie>> SearchMoviesByUserIdAsync(int userId, string searchTerm);
    }
}
