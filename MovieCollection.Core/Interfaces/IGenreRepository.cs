using MovieCollection.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Core.Interfaces
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetAllGenresAsync();
        Task<List<Genre>> GetGenresByIdsAsync(List<int> genreIds);
    }
}
