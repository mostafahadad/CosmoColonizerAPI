using CosmoColonizerAPI.Data;
using CosmoColonizerAPI.Data.DTOs.User;
using CosmoColonizerAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CosmoColonizerAPI.Services.Users
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new KeyNotFoundException($"No user with ID {id} was found");

            return user;
        }

        public async Task<User> AddAsync(UserDTO userDto)
        {
            var existingUser = await _context.Users.FindAsync(userDto.Id);

            if (existingUser != null)
            {
                return existingUser;
            }

            var user = new User
            {
                Id = userDto.Id,
                Name = userDto.Name,
                PlanetId = userDto.PlanetId
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
       