using CosmoColonizerAPI.Data;
using CosmoColonizerAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace CosmoColonizerAPI.Services.Planets
{
    public class PlanetsService : IPlanetsService
    {
        private readonly ApplicationDbContext _context;
        public PlanetsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Planet>> GetAllAsync()
        {
            return await _context.Planets.ToListAsync();
        }

        public async Task<Planet> GetByIdAsync(int id)
        {
            var planet = await _context.Planets.FindAsync(id);
            if (planet == null) throw new KeyNotFoundException($"No planet with ID {id} was found");

            return planet;
        }

        public async Task<Planet> AddAsync(Planet planet)
        {
            if (planet == null) throw new ArgumentNullException("Provided planet is null");

            await _context.Planets.AddAsync(planet);
            await _context.SaveChangesAsync();

            return planet;
        }

        public async Task<Planet> UpdateAsync(Planet planet)
        {
            if (planet == null) throw new ArgumentNullException(nameof(planet));

            var existingPlanet = await _context.Planets.FindAsync(planet.Id);
            if (existingPlanet == null) throw new KeyNotFoundException($"No planet with ID {planet.Id} was found");

            existingPlanet.Name = planet.Name;

            await _context.SaveChangesAsync();

            return existingPlanet; 
        }


        public async Task DeleteByIdAsync(int id)
        {
            var planet = await _context.Planets.FindAsync(id);

            if (planet == null) throw new KeyNotFoundException($"No planet with ID {id} was found");

            _context.Planets.Remove(planet);
            await _context.SaveChangesAsync();
        }
    }
}
