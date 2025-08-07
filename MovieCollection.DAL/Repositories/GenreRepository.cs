using Microsoft.EntityFrameworkCore;
using MovieCollection.Core.Interfaces;
using MovieCollection.Core.Models;
using MovieCollection.DAL.Data;

public class GenreRepository : IGenreRepository
{
    private readonly MovieContext _context;
    public GenreRepository(MovieContext context)
    {
        _context = context;
    }
    public async Task<List<Genre>> GetAllGenresAsync()
    {
        return await _context.Genres.ToListAsync();
    }
    public async Task<List<Genre>> GetGenresByIdsAsync(List<int> genreIds)
    {
        return await _context.Genres
                             .Where(g => genreIds.Contains(g.GenreId))
                             .ToListAsync();
    }

}
