using CosmoColonizerAPI.Data.Entities;

namespace CosmoColonizerAPI.Services.Planets
{
    public interface IPlanetsService
    {
        Task<ICollection<Planet>> GetAllAsync();
        Task<Planet> GetByIdAsync(int id);
        Task<Planet> AddAsync(Planet planet);
        Task<Planet> UpdateAsync(Planet planet);    
        Task DeleteByIdAsync(int id);
    }
}
