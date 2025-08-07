using MovieCollection.Core.Models;
using MyMovieCollection.UI;

public class UserMenuUI
{
    private readonly MovieUI movieUI;

    public UserMenuUI(MovieUI movieUI)
    {
        this.movieUI = movieUI;
    }
    public async Task ShowMenuAsync(User loggedInUser)
    {
        bool isRunning = true;

        while (isRunning)
        {
            Console.Clear();
            Console.WriteLine(
                "=====================================\n" +
                $" Welcome, {loggedInUser.FullName} \n" +
                "=====================================\n\n" +
                "1. Add a New Movie\n" +
                "2. View My Movies\n" +
                "3. Edit a Movie\n" +
                "4. Delete a Movie\n" +
                "5. Search a Movie\n" +
                "6. Logout\n");

            Console.Write("Your Choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await movieUI.AddMovieAsync();
                    break;

                case "2":
                    await movieUI.ViewMovieAsync();
                    break;

                case "3":
                    await movieUI.UpdateMovieAsync();
                    break;

                case "4":
                    await movieUI.DeleteMovieAsync();
                    break;

                case "5":
                    await movieUI.SearchMovieAsync();
                    break;

                case "6":
                    Console.WriteLine("Logging out...");
                    Thread.Sleep(1300);
                    isRunning = false;
                    break;

                default:
                    Console.WriteLine("Invalid Choice. Please try again.");
                    Console.ReadKey();
                    break;
            }

        }
    }
}
