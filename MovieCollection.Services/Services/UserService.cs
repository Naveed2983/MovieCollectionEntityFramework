using MovieCollection.Core.Interfaces;
using MovieCollection.Core.Models;
using MovieCollection.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task AddUserAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.UserName)||
                string.IsNullOrWhiteSpace(user.FullName)||
                string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentException("All fields are required");
            }
            if (await _userRepository.UserExistsByUsernameAsync(user.UserName))
            {
                throw new ArgumentException("Username Already Exists");
            }
            if (user.Password.Length<8)
            {
                throw new ArgumentException("Password must be atleast 8 characters long.");
            }
            await _userRepository.AddUserAsync(user);
        }
        public async Task<User?> LoginAsync(string username, string password)
        {
            if(string.IsNullOrWhiteSpace(username)|| string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Username and Password are required.");
            }
            var user= await _userRepository.GetUserByCredentialsAsync(username, password);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }
            return user;
        }
    }
}