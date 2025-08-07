using MovieCollection.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Core.Models
{
    public class MovieRating
    {
        public int MovieId { get; set; }
        public double ImdbRating { get; set; }
        public int Votes { get; set; }
        public  AgeRating AgeRating { get; set; }
        public Movie? Movie { get; set; }
    }
}
