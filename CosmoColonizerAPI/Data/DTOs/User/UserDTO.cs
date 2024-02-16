using CosmoColonizerAPI.Data.Entities;

namespace CosmoColonizerAPI.Data.DTOs.User
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? PlanetId { get; set; }
    }
}
