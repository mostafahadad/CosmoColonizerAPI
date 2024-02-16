using CosmoColonizerAPI.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmoColonizerAPI.Services.Planets
{
    public interface IPlanetsService
    {
        Task<ICollection<Planet>> GetAllAsync();
        Task<Planet> GetByIdAsync(int id);
        Task<IEnumerable<Planet>> GetByIdsAsync(List<int> Ids);
        Task<Planet> AddAsync(Planet planet);
        Task<Planet> UpdateAsync(Planet planet);    
        Task DeleteByIdAsync(int id);
        Task<(Planet BestPlanet, string Explanation)> SuggestBestPlanetForColonizationAsync();
    }
}
