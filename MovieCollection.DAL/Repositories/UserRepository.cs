using Microsoft.EntityFrameworkCore;
using MovieCollection.Core.Interfaces;
using MovieCollection.Core.Models;
using MovieCollection.DAL.Data;

namespace MovieCollection.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MovieContext context;
        public UserRepository(MovieContext _context)
        {
            context = _context;
        }
        public async Task AddUserAsync(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }
        public async Task<bool> UserExistsByUsernameAsync(string username)
        {
            return await context.Users.AnyAsync(u => u.UserName == username);
        }
        public async Task<User?> GetUserByCredentialsAsync(string username, string password)
        {
            return await context.Users
            .FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
        }
    }
}
