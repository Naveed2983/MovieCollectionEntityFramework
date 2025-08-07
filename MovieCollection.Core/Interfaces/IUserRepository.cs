using MovieCollection.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Core.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<bool> UserExistsByUsernameAsync(string username);
        Task<User?> GetUserByCredentialsAsync(string username, string password);
    }
}
