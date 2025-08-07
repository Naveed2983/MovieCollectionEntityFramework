using MovieCollection.Core.Interfaces;
using MovieCollection.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieCollection.UI
{
    public class UserUI
    {
        private readonly IUserService _userService;
        private readonly IMovieService _movieService;
        private readonly IGenreService _genreService;
        public UserUI(IUserService userService, IMovieService movieService, IGenreService genreService)
        {
            _userService = userService;
            _movieService = movieService;
            _genreService = genreService;
        }
        public async Task RegisterUserAsync()
        {
            Console.Clear();
            Console.WriteLine("=========== Register ===========");

            Console.Write("Full Name: ");
            var fullName = Console.ReadLine();

            Console.Write("Username: ");
            var username = Console.ReadLine();

            Console.Write("Password: ");
            var password = Console.ReadLine();

            var user = new User
            {
                FullName = fullName,
                UserName = username,
                Password = password
            };

            try
            {
                await _userService.AddUserAsync(user);
                Console.WriteLine("\n Registration successful!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\n Error: {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("\n An unexpected error occurred. Please try again later.");
            }

            Console.WriteLine("\n(Press any key to return to main menu...)");
            Console.ReadKey();
        }
        public async Task LoginUserAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Login ===");
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();

            try
            {
                var user = await _userService.LoginAsync(username, password);
                Console.WriteLine($"Welcome back, {user.FullName}!");

                var movieUI = new MovieUI(_movieService, _genreService, user.UserId);
                var userMenuUI = new UserMenuUI(movieUI);

                await userMenuUI.ShowMenuAsync(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
