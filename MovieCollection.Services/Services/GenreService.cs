using MovieCollection.Core.Interfaces;
using MovieCollection.Core.Models;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }
    public async Task<List<Genre>> GetAllGenresAsync()
    {
        return await _genreRepository.GetAllGenresAsync();
    }
    public Task<List<Genre>> GetGenresByIdsAsync(List<int> genreIds)
    {
        return _genreRepository.GetGenresByIdsAsync(genreIds);
    }

}
