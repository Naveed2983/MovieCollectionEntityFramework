using MovieCollection.Core.Interfaces;
using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieCollection.Services.Services;
using MovieCollection.Core.Models;
using MovieCollection.Core.Enums;
using Microsoft.IdentityModel.Tokens;

namespace MyMovieCollection.Services.Tests.Services
{
    [TestFixture]
    public class MovieServiceTests
    {
        private Mock<IMovieRepository> _movieRepositoryMock;
        private Mock<IGenreRepository> _genreRepositoryMock;
        private IMovieService _movieService;

        [SetUp]
        public void Setup()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _genreRepositoryMock = new Mock<IGenreRepository>();

            _movieService = new MovieService(_movieRepositoryMock.Object, _genreRepositoryMock.Object);
        }

        [Test]
        public async Task AddMovieAsync_ValidMovie_ShouldCallRepository()
        {
            var genres = new List<Genre> { new Genre { GenreId = 1, GenreName = "Action" } };
            _genreRepositoryMock.Setup(r => r.GetAllGenresAsync())
                .ReturnsAsync(genres);

            var movie = new Movie
            {
                Title = "Inception",
                Duration = 120,
                ReleaseYear = 2010,
                Genres = genres,
                Rating = new MovieRating { ImdbRating = 9, Votes = 1000, AgeRating = AgeRating.PG13 }
            };

            await _movieService.AddMovieAsync(movie);

            _movieRepositoryMock.Verify(r => r.AddMovieAsync(movie), Times.Once);
        }

        [Test]
        [TestCase(" ")]
        [TestCase("")]
        [TestCase(null)]
        public async Task AddMovieAsync_TitleWhiteSpaceOrNull_ThrowsExceptiony(string input)
        {
            var genres = new List<Genre> { new Genre { GenreId = 1, GenreName = "Action" } };
            _genreRepositoryMock.Setup(r => r.GetAllGenresAsync())
                .ReturnsAsync(genres);

            var movie = new Movie
            {
                Title = " ",
                Duration = 120,
                ReleaseYear = 2010,
                Genres = genres,
                Rating = new MovieRating { ImdbRating = 9, Votes = 1000, AgeRating = AgeRating.PG13 }
            };

            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _movieService.AddMovieAsync(movie));

            Assert.That(ex.Message, Is.EqualTo("Movie title is required."));
        }

        [Test]
        public async Task AddMovieAsync_DurationNegative_ThrowsException()
        {
            var genres = new List<Genre> { new Genre { GenreId = 1, GenreName = "Action" } };
            _genreRepositoryMock.Setup(r => r.GetAllGenresAsync())
                .ReturnsAsync(genres);

            var movie = new Movie
            {
                Title = "Inception",
                Duration = -1,
                ReleaseYear = 2010,
                Genres = genres,
                Rating = new MovieRating { ImdbRating = 9, Votes = 1000, AgeRating = AgeRating.PG13 }
            };

            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _movieService.AddMovieAsync(movie));

            Assert.That(ex.Message, Is.EqualTo("Duration must be positive."));
        }

        [Test]
        [TestCase(2027)]
        [TestCase(1700)]
        [TestCase(-1)]
        public async Task AddMovieAsync_InvalidReleaseYear_ThrowsException(int input)
        {
            var genres = new List<Genre> { new Genre { GenreId = 1, GenreName = "Action" } };
            _genreRepositoryMock.Setup(r => r.GetAllGenresAsync())
                .ReturnsAsync(genres);

            var movie = new Movie
            {
                Title = "Inception",
                Duration = 120,
                ReleaseYear = input,
                Genres = genres,
                Rating = new MovieRating { ImdbRating = 9, Votes = 1000, AgeRating = AgeRating.PG13 }
            };

            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _movieService.AddMovieAsync(movie));

            Assert.That(ex.Message, Is.EqualTo("Release year is invalid."));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(11)]
        public async Task AddMovieAsync_InvalidImdbRating_ThrowsException(double input)
        {
            var genres = new List<Genre> { new Genre { GenreId = 1, GenreName = "Action" } };
            _genreRepositoryMock.Setup(r => r.GetAllGenresAsync())
                .ReturnsAsync(genres);

            var movie = new Movie
            {
                Title = "Inception",
                Duration = 120,
                ReleaseYear = 2010,
                Genres = genres,
                Rating = new MovieRating { ImdbRating = input, Votes = 1000, AgeRating = AgeRating.PG13 }
            };

            movie.Genres = null;

            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _movieService.AddMovieAsync(movie));

            Assert.That(ex.Message, Is.EqualTo("IMDb rating must be between 0 and 10."));
        }

        [Test]
        [TestCase(-1)]
        public async Task AddMovieAsync_InvalidVotes_ThrowsException(int input)
        {
            var genres = new List<Genre> { new Genre { GenreId = 1, GenreName = "Action" } };
            _genreRepositoryMock.Setup(r => r.GetAllGenresAsync())
                .ReturnsAsync(genres);

            var movie = new Movie
            {
                Title = "Inception",
                Duration = 120,
                ReleaseYear = 2010,
                Genres = genres,
                Rating = new MovieRating { ImdbRating = 9, Votes = input, AgeRating = AgeRating.PG13 }
            };



            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _movieService.AddMovieAsync(movie));

            Assert.That(ex.Message, Is.EqualTo("Votes cannot be negative."));
        }

        [Test]
        public async Task AddMovieAsync_GenresNull_ThrowsException()
        {
            var movie = new Movie
            {
                Title = "Inception",
                Duration = 120,
                ReleaseYear = 2010,
                Genres = null,
                Rating = new MovieRating { ImdbRating = 9, Votes = 1200, AgeRating = AgeRating.PG13 }
            };

            var ex = Assert.ThrowsAsync<NullReferenceException>(async () =>
                await _movieService.AddMovieAsync(movie));
        }

        [Test]
        public async Task AddMovieAsync_MissingRating_ShouldThrowException()
        {
            var genres = new List<Genre> { new Genre { GenreId = 1, GenreName = "Action" } };
            _genreRepositoryMock.Setup(r => r.GetAllGenresAsync())
                .ReturnsAsync(genres);

            var movie = new Movie
            {
                Title = "Interstellar",
                Duration = 169,
                ReleaseYear = 2014,
                Genres = genres,
                Rating = null
            };

            var ex = Assert.ThrowsAsync<Exception>(async () => await _movieService.AddMovieAsync(movie));

            Assert.That(ex.Message, Is.EqualTo("Rating is required."));
        }

        [Test]
        public async Task DeleteMovieAsync_ValidMovieId_ShouldCallRepository()
        {
            int MovieId = 1;
            int UserId = 10;
            var movie = new Movie { MovieId = MovieId, UserId = UserId, ReleaseYear = 2010, Duration = 120, Title = "Inception" };

            _movieRepositoryMock.Setup(r=>r.GetMovieByIdAsync(MovieId,UserId)).ReturnsAsync(movie);

            await _movieService.DeleteMovieAsync(MovieId, UserId);

            _movieRepositoryMock.Verify(r=>r.DeleteMovieAsync(MovieId,UserId), Times.Once());  
        }

        [Test]
        public async Task DeleteMovieAsync_InvalidMovieId_ShouldNotCallRepository()
        {

            _movieRepositoryMock.Setup(r => r.GetMovieByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((Movie)null);

            var ex= Assert.ThrowsAsync<Exception>(()=>_movieService.DeleteMovieAsync(It.IsAny<int>(), It.IsAny<int>()));
            Assert.That(ex.Message, Is.EqualTo("Invalid Movie ID."));

            _movieRepositoryMock.Verify(r=>r.DeleteMovieAsync(It.IsAny<int>(), It.IsAny<int>()),Times.Never());
        }
        [Test]
        public async Task GetAllMoviesByUserIdAsync_WhenMoviesExist_ShouldReturnMovies()
        {
            int UserId = 1;
            var movie= new List<Movie>()
            {
                new Movie{MovieId=1,UserId=UserId},
                new Movie{MovieId=2,UserId=UserId},
            };
            _movieRepositoryMock.Setup(r=>r.GetAllMoviesByUserIdAsync(UserId)).ReturnsAsync(movie);

            var result= await _movieService.GetAllMoviesByUserIdAsync(UserId);

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result, Is.Not.Null);
           
        }

        [Test]
        public async Task GetAllMoviesByUserIdAsync_WhenMoviesEmpty_ShouldThrowInvalidOperationException()
        {
            int UserId = 1;
            var movie = new List<Movie>();
            _movieRepositoryMock.Setup(r => r.GetAllMoviesByUserIdAsync(UserId)).ReturnsAsync(movie);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _movieService.GetAllMoviesByUserIdAsync(UserId));

            Assert.That(ex.Message, Is.EqualTo("No movies found."));
        }

        [Test]
        public async Task GetAllMoviesByUserIdAsync_WhenMovieNull_ShouldThrowInvalidOperationException()
        {

            int UserId = 1;
            var movie = new List<Movie>();
            _movieRepositoryMock.Setup(r => r.GetAllMoviesByUserIdAsync(UserId)).ReturnsAsync((List<Movie>?)null);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _movieService.GetAllMoviesByUserIdAsync(UserId));

            Assert.That(ex.Message, Is.EqualTo("No movies found."));
        }

        public async Task GetMovieByIdAsync_ValidIds_ReturnsMovie()
        {
            int movieId = 1;
            int userId = 100;

            var expectedMovie = new Movie { MovieId = movieId, Title = "Inception", UserId = userId };
            _movieRepositoryMock.Setup(r => r.GetMovieByIdAsync(movieId, userId))
                .ReturnsAsync(expectedMovie);

            var result = await _movieService.GetMovieByIdAsync(movieId, userId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.MovieId, Is.EqualTo(movieId));
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.Title, Is.EqualTo("Inception"));
        }

      
        public async Task GetMovieByIdAsync_InvalidIds_ReturnsNull()
        {
            _movieRepositoryMock.Setup(r => r.GetMovieByIdAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((Movie?)null);

            var result = await _movieService.GetMovieByIdAsync(It.IsAny<int>(), It.IsAny<int>());

            Assert.That(result,Is.Null);
        }

        [Test]
        public async Task SearchMoviesByUserIdAsync_WithValidSearch_ReturnsMovies()
        {
            var userId = 1;
            var searchTerm = "Inception";
            var expectedMovies = new List<Movie> { new Movie { MovieId = 1, Title = "Inception" } };

            _movieRepositoryMock.Setup(r => r.SearchMoviesByUserIdAsync(userId, searchTerm))
                    .ReturnsAsync(expectedMovies);

            var result = await _movieService.SearchMoviesByUserIdAsync(userId, searchTerm);

            Assert.That(result, Is.Not.Null);
            Assert.That(1, Is.EqualTo(result.Count));
            Assert.That(result[0].Title, Is.EqualTo("Inception"));
        }

        [Test]
        public void SearchMoviesByUserIdAsync_NoMoviesFound_ThrowsException()
        {
            _movieRepositoryMock.Setup(r => r.SearchMoviesByUserIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                    .ReturnsAsync(new List<Movie>());

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _movieService.SearchMoviesByUserIdAsync(It.IsAny<int>(), It.IsAny<string>()));
        }

        [Test]
        public void UpdateMovieAsync_ValidTitle_ShouldUpdate()
        {
            var existingMovie = new Movie { MovieId = 1, UserId = 1, Title = "Inception" };
            _movieRepositoryMock.Setup(r => r.GetMovieByIdAsync(1, 1)).ReturnsAsync(existingMovie);

            var updateMovie = new Movie { MovieId = 1, UserId = 1, Title = "new title" };

            _movieService.UpdateMovieAsync(updateMovie);

            Assert.That(existingMovie.Title, Is.EqualTo("new title"));

            _movieRepositoryMock.Verify(r=>r.UpdateMovieAsync(existingMovie),Times.Once);
        }
        [Test]
        public void UpdateMovieAsync_MovieNotFound_ShouldThrowException()
        {
            _movieRepositoryMock.Setup(r => r.GetMovieByIdAsync(1, 1)).ReturnsAsync((Movie?)null);
            var updateMovie = new Movie { MovieId = 1, UserId = 1};
            var ex = Assert.ThrowsAsync<Exception>(async () => await _movieService.UpdateMovieAsync(updateMovie));
            Assert.That(ex.Message, Is.EqualTo("Movie not found."));
        }

        [Test]
        public void UpdateMovieAsync_GenresUpdated_ShouldUpdateGenres()
        {
            var existingMovie = new Movie { MovieId=1, UserId = 1, Genres= new List<Genre>()};
            _movieRepositoryMock.Setup(r=>r.GetMovieByIdAsync(1,1)).ReturnsAsync(existingMovie);
            var updateMovie = new Movie
            {
                MovieId = 1,
                UserId = 1,
                Genres = new List<Genre>()
                {
                    new Genre
                    {
                        GenreId = 1,
                        GenreName="Action"
                    }
                }
            };

            _genreRepositoryMock.Setup(g=>g.GetGenresByIdsAsync(It.IsAny<List<int>>())).ReturnsAsync(new List<Genre> {  new Genre {  GenreId = 1,GenreName="Action" } });
            _movieService.UpdateMovieAsync(updateMovie);
            Assert.That(existingMovie.Genres.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task UpdateMovieAsync_RatingsUpdated_ShouldUpdateRatings()
        {
            var existingMovie = new Movie { MovieId = 1, UserId = 1, Rating= new MovieRating() };
            _movieRepositoryMock.Setup(r=>r.GetMovieByIdAsync(1,1)).ReturnsAsync(existingMovie);

            var updateMovie = new Movie
            {
                MovieId = 1,
                UserId = 1,
                Rating = new MovieRating() { AgeRating = AgeRating.PG13, ImdbRating = 9.3, Votes = 1200 }
            };

            await _movieService.UpdateMovieAsync(updateMovie);

            Assert.That(existingMovie.Rating, Is.Not.Null);
            Assert.That(existingMovie.Rating.Votes, Is.EqualTo(1200));
            Assert.That(existingMovie.Rating.AgeRating, Is.EqualTo(AgeRating.PG13));


        }
    }
}
