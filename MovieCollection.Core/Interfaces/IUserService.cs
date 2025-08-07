using MovieCollection.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Core.Interfaces
{
    public interface IUserService
    {
        Task AddUserAsync(User user);
        Task<User?> LoginAsync(string username, string password);
    }
}
