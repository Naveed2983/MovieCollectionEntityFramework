using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Core.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public int Duration { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public MovieRating Rating { get; set; }
    }
}
