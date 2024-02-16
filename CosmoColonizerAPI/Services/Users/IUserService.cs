using CosmoColonizerAPI.Data.DTOs.User;
using CosmoColonizerAPI.Data.Entities;

namespace CosmoColonizerAPI.Services.Users
{
    public interface IUserService
    {
        Task<ICollection<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid id);
        Task<User> AddAsync(UserDTO userDto);
        
    }
}
