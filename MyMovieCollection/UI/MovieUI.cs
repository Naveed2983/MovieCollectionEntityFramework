using MovieCollection.Core.Enums;
using MovieCollection.Core.Interfaces;
using MovieCollection.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMovieCollection.UI
{
    public class MovieUI
    {
        private readonly IMovieService _movieService;
        private readonly IGenreService _genreService;
        private readonly int _userId;

        public MovieUI(IMovieService movieService, IGenreService genreService, int userId)
        {
            _movieService = movieService;
            _genreService = genreService;
            _userId = userId;
        }

        public async Task AddMovieAsync()
        {
            Console.Clear();
            Console.WriteLine("==== Add Movie ====\n");

            string title;
            while (true)
            {
                Console.Write("Title: ");
                title = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(title))
                    break;

                Console.WriteLine("Title is required. Please try again.");
            }

            int duration;
            while (true)
            {
                Console.Write("Duration (in minutes): ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out duration) && duration > 0)
                    break;

                Console.WriteLine("Please enter a valid positive number for duration.");
            }

            int releaseYear;
            while (true)
            {
                Console.Write("Release Year: ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out releaseYear) &&
                    releaseYear >= 1900 &&
                    releaseYear <= DateTime.Now.Year)
                    break;

                Console.WriteLine($"Enter a valid release year (1900 to {DateTime.Now.Year}).");
            }

            // Genres
            Console.WriteLine("\nAvailable Genres:");
            var genres = await _genreService.GetAllGenresAsync();
            for (int i = 0; i < genres.Count; i++)
                Console.WriteLine($"{i + 1}. {genres[i].GenreName}");

            List<Genre> selectedGenres = new();
            while (true)
            {
                Console.Write("\nSelect genres (e.g., 1,3,5): ");
                var input = Console.ReadLine();

                try
                {
                    var selectedIndices = input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                               .Select(s => int.Parse(s.Trim()) - 1)
                                               .ToList();

                    if (selectedIndices.Any(i => i < 0 || i >= genres.Count))
                        throw new Exception();

                    selectedGenres = selectedIndices.Select(i => genres[i]).ToList();
                    break;
                }
                catch
                {
                    Console.WriteLine("Invalid genre selection. Please enter valid numbers separated by commas.");
                }
            }

            double imdbRating;
            while (true)
            {
                Console.Write("\nIMDb Rating (0 to 10): ");
                var input = Console.ReadLine();
                if (double.TryParse(input, out imdbRating) && imdbRating >= 0 && imdbRating <= 10)
                    break;

                Console.WriteLine("Enter a valid IMDb rating between 0 and 10.");
            }

            int votes;
            while (true)
            {
                Console.Write("Total Votes: ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out votes) && votes >= 0)
                    break;

                Console.WriteLine("Please enter a non-negative number for votes.");
            }

            Console.WriteLine("Select Age Rating:");
            foreach (var age in Enum.GetValues(typeof(AgeRating)))
                Console.WriteLine($"{(int)age}. {age}");

            AgeRating ageRating;
            while (true)
            {
                Console.Write("Enter choice: ");
                var input = Console.ReadLine();

                if (int.TryParse(input, out int ageChoice) && Enum.IsDefined(typeof(AgeRating), ageChoice))
                {
                    ageRating = (AgeRating)ageChoice;
                    break;
                }
                Console.WriteLine("Invalid age rating. Try again.");
            }

            var movie = new Movie
            {
                Title = title,
                Duration = duration,
                ReleaseYear = releaseYear,
                UserId = _userId,
                Genres = selectedGenres,
                Rating = new MovieRating
                {
                    ImdbRating = imdbRating,
                    Votes = votes,
                    AgeRating = ageRating
                }
            };

            try
            {
                await _movieService.AddMovieAsync(movie);
                Console.WriteLine("\nMovie added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

            Console.WriteLine("\n(Press any key to return to menu...)");
            Console.ReadKey();
        }
        public async Task ViewMovieAsync()
        {
            Console.Clear();
            Console.WriteLine("======= Your Movies =======");

            try
            {
                var movies = await _movieService.GetAllMoviesByUserIdAsync(_userId);

                foreach (var movie in movies)
                {
                    Console.WriteLine($"\nTitle: {movie.Title}");
                    Console.WriteLine($"Duration: {movie.Duration} minutes");
                    Console.WriteLine($"Release Year: {movie.ReleaseYear}");
                    Console.WriteLine($"Age Rating: {movie.Rating.AgeRating}");
                    Console.WriteLine($"IMDB Rating: {movie.Rating.ImdbRating}");
                    Console.WriteLine($"Votes: {movie.Rating.Votes}");
                    Console.WriteLine("Genres: " + string.Join(", ", movie.Genres.Select(g => g.GenreName)));
                    Console.WriteLine(new string('-', 40));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
        public async Task UpdateMovieAsync()
        {
            Console.Clear();
            Console.WriteLine("===== Update Movie =====\n");

            var movies = await _movieService.GetAllMoviesByUserIdAsync(_userId);
            if (movies.Count == 0)
            {
                Console.WriteLine("No movies found.");
                Console.ReadKey();
                return;
            }

            foreach (var m in movies)
                Console.WriteLine($"{m.MovieId}. {m.Title}");

            int movieId;
            while (true)
            {
                Console.Write("\nEnter Movie ID to update: ");
                if (int.TryParse(Console.ReadLine(), out movieId) &&
                    movies.Any(m => m.MovieId == movieId))
                    break;
                Console.WriteLine("Invalid Movie ID. Please try again.");
            }

            var movie = await _movieService.GetMovieByIdAsync(movieId, _userId);
            if (movie == null)
            {
                Console.WriteLine("Movie not found.");
                return;
            }

            Console.WriteLine("\n(Leave blank to keep current value)\n");

            Console.Write($"Title ({movie.Title}): ");
            string titleInput = Console.ReadLine();
            while (!string.IsNullOrWhiteSpace(titleInput) && titleInput.Length < 2)
            {
                Console.WriteLine("Title must be at least 2 characters.");
                Console.Write("Enter again: ");
                titleInput = Console.ReadLine();
            }
            if (!string.IsNullOrWhiteSpace(titleInput))
                movie.Title = titleInput;

            Console.Write($"Duration in minutes ({movie.Duration}): ");
            string durationInput = Console.ReadLine();
            while (!string.IsNullOrWhiteSpace(durationInput) &&
                   (!int.TryParse(durationInput, out int d) || d <= 0))
            {
                Console.WriteLine("Invalid duration. Must be a positive number.");
                Console.Write("Enter again: ");
                durationInput = Console.ReadLine();
            }
            if (int.TryParse(durationInput, out int duration))
                movie.Duration = duration;

            Console.Write($"Release Year ({movie.ReleaseYear}): ");
            string yearInput = Console.ReadLine();
            while (!string.IsNullOrWhiteSpace(yearInput) &&
                   (!int.TryParse(yearInput, out int y) || y < 1800 || y > DateTime.Now.Year))
            {
                Console.WriteLine($"Invalid year. Must be between 1800 and {DateTime.Now.Year}.");
                Console.Write("Enter again: ");
                yearInput = Console.ReadLine();
            }
            if (int.TryParse(yearInput, out int year))
                movie.ReleaseYear = year;

            Console.Write("Do you want to edit genres? (y/n): ");
            string genreChoice = Console.ReadLine()?.Trim().ToLower();
            if (genreChoice == "y")
            {
                var allGenres = await _genreService.GetAllGenresAsync();
                for (int i = 0; i < allGenres.Count; i++)
                    Console.WriteLine($"{i + 1}. {allGenres[i].GenreName}");

                List<Genre> selectedGenres;
                while (true)
                {
                    Console.Write("Select genres (e.g., 1,3): ");
                    var selected = Console.ReadLine();
                    var indices = selected.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                          .Select(s => s.Trim());

                    selectedGenres = new();
                    bool allValid = true;

                    foreach (var indexStr in indices)
                    {
                        if (int.TryParse(indexStr, out int index) &&
                            index > 0 && index <= allGenres.Count)
                        {
                            selectedGenres.Add(allGenres[index - 1]);
                        }
                        else
                        {
                            Console.WriteLine("Invalid genre selection. Please try again.");
                            allValid = false;
                            break;
                        }
                    }

                    if (allValid) break;
                }

                movie.Genres = selectedGenres;
            }

            Console.Write("Do you want to update rating? (y/n): ");
            string ratingChoice = Console.ReadLine()?.Trim().ToLower();
            if (ratingChoice == "y")
            {
                var rating = new MovieRating();

                string imdbInput;
                while (true)
                {
                    Console.Write($"IMDB Rating ({movie.Rating?.ImdbRating ?? 0}): ");
                    imdbInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(imdbInput)) break;
                    if (double.TryParse(imdbInput, out double imdb) && imdb >= 0 && imdb <= 10)
                    {
                        rating.ImdbRating = imdb;
                        break;
                    }
                    Console.WriteLine("Invalid rating. Must be between 0 and 10.");
                }

                string votesInput;
                while (true)
                {
                    Console.Write($"Votes ({movie.Rating?.Votes ?? 0}): ");
                    votesInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(votesInput)) break;
                    if (int.TryParse(votesInput, out int votes) && votes >= 0)
                    {
                        rating.Votes = votes;
                        break;
                    }
                    Console.WriteLine("Invalid votes. Must be a non-negative number.");
                }

                var ageValues = Enum.GetValues(typeof(AgeRating)).Cast<AgeRating>().ToList();
                foreach (var val in ageValues)
                    Console.WriteLine($"{(int)val}. {val}");

                while (true)
                {
                    Console.Write($"Age Rating ({movie.Rating?.AgeRating ?? AgeRating.PG}): ");
                    var ageInput = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(ageInput)) break;
                    if (int.TryParse(ageInput, out int ageVal) &&
                        Enum.IsDefined(typeof(AgeRating), ageVal))
                    {
                        rating.AgeRating = (AgeRating)ageVal;
                        break;
                    }
                    Console.WriteLine("Invalid age rating. Try again.");
                }

                movie.Rating = rating;
            }

            try
            {
                await _movieService.UpdateMovieAsync(movie);
                Console.WriteLine("\nMovie updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\n(Press any key to return...)");
            Console.ReadKey();
        }
        public async Task DeleteMovieAsync()
        {
            Console.Clear();
            Console.WriteLine("==== Delete a Movie ====\n");

            var movies = await _movieService.GetAllMoviesByUserIdAsync(_userId);

            if (movies == null || movies.Count == 0)
            {
                Console.WriteLine("No movies found for your account.");
                Console.WriteLine("\n(Press any key to return to menu...)");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Your Movies:");
            foreach (var movie in movies)
            {
                Console.WriteLine($"ID: {movie.MovieId} | Title: {movie.Title}");
            }

            Console.Write("\nEnter the ID of the movie to delete: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out int movieId))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                return;
            }

            try
            {
                await _movieService.DeleteMovieAsync(movieId, _userId);
                Console.WriteLine("Movie deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\n(Press any key to return to menu...)");
            Console.ReadKey();
        }
        public async Task SearchMovieAsync()
        {
            Console.Clear();
            Console.WriteLine("======= Search Movies =======");

            Console.Write("Enter your Keyword: ");
            var keyword = Console.ReadLine();

            try
            {
                var movies = await _movieService.SearchMoviesByUserIdAsync(_userId,keyword);

                foreach (var movie in movies)
                {
                    Console.WriteLine($"\nTitle: {movie.Title}");
                    Console.WriteLine($"Duration: {movie.Duration} minutes");
                    Console.WriteLine($"Release Year: {movie.ReleaseYear}");
                    Console.WriteLine($"Age Rating: {movie.Rating.AgeRating}");
                    Console.WriteLine($"IMDB Rating: {movie.Rating.ImdbRating}");
                    Console.WriteLine($"Votes: {movie.Rating.Votes}");
                    Console.WriteLine("Genres: " + string.Join(", ", movie.Genres.Select(g => g.GenreName)));
                    Console.WriteLine(new string('-', 40));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
            }
    }
}
