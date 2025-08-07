using MovieCollection.DAL.Data;
using MovieCollection.DAL.Repositories;
using MovieCollection.Services.Services;
using MyMovieCollection.UI;

var context = new MovieContext();

if (context.Database.CanConnect())
{
    Console.WriteLine("Connected to the database!");
}
else
{
    Console.WriteLine("Could not connect to the database.");
}

var userRepository = new UserRepository(context);
var movieRepository = new MovieRepository(context);
var genreRepository = new GenreRepository(context);


var userService = new UserService(userRepository);
var movieService = new MovieService(movieRepository, genreRepository);
var genreService = new GenreService(genreRepository);

var userUI = new UserUI(userService, movieService, genreService);

var loopRunning = true;
while (loopRunning)
{
    Console.Clear();
    Console.WriteLine(
        "=====================================\n" +
        "        MOVIE COLLECTION MANAGER      \n" +
        "=====================================\n"
    );
    Console.WriteLine(
        "1. Register\n" +
        "2. Login   \n" +
        "3. Exit    \n");

    Console.Write("Your Choice: ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            await userUI.RegisterUserAsync();
            break;
        case "2":
            await userUI.LoginUserAsync();
            Console.WriteLine("(Press any key to continue...)");
            Console.ReadKey();
            break;
        case "3":
            loopRunning = false;
            break;
        default:
            Console.WriteLine("Invalid choice! Please enter a valid option (1-3).");
            Console.WriteLine("(Press any key to continue...)");
            Console.ReadKey();
            break;
    }
}
