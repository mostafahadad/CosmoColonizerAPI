using CosmoColonizerAPI.Data;
using CosmoColonizerAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

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

        public async Task<IEnumerable<Planet>> GetByIdsAsync(List<int> planetIds)
        {
            return await _context.Planets
                                 .Where(planet => planetIds.Contains(planet.Id))
                                 .ToListAsync();
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

            if (planet.ImageUrl != null)
                existingPlanet.ImageUrl = planet.ImageUrl;
            if (planet.AtmosphericPressure != null)
                existingPlanet.AtmosphericPressure = planet.AtmosphericPressure;
            if (planet.Temperature != null)
                existingPlanet.Temperature = planet.Temperature;
            if (planet.WaterVolume != null)
                existingPlanet.WaterVolume = planet.WaterVolume;
            if (planet.OxygenVolume != null)
                existingPlanet.OxygenVolume = planet.OxygenVolume;
            if (planet.Gravity != null)
                existingPlanet.Gravity = planet.Gravity;

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

        public async Task<(Planet BestPlanet, string Explanation)> SuggestBestPlanetForColonizationAsync()
        {
            var planets = await GetAllAsync();
            Planet bestPlanet = null;
            double bestScore = 0;
            List<string> bestReasons = new List<string>();

            foreach (var planet in planets)
            {
                var (score, reasons) = CalculatePlanetScore(planet);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestPlanet = planet;
                    bestReasons = reasons;
                }
            }
            string explanation = $"{bestPlanet?.Name} shows promising conditions for colonization:\n- {string.Join(".\n- ", bestReasons)}.";

            return (bestPlanet, explanation);
        }

        private (double Score, List<string> Reasons) CalculatePlanetScore(Planet planet)
        {
            double score = 0;
            List<string> reasons = new List<string>();

            if (planet.OxygenVolume.HasValue)
            {
                double oxygenScore = (planet.OxygenVolume.Value / 100) * 20; // Assuming 100 is the ideal volume for simplicity
                score += oxygenScore;
                reasons.Add($"Oxygen volume at {planet.OxygenVolume.Value} is considered adequate");
            }

            if (planet.WaterVolume.HasValue)
            {
                double waterScore = (planet.WaterVolume.Value / 100) * 20; // Assuming 100 is the ideal volume
                score += waterScore;
                reasons.Add($"Water volume at {planet.WaterVolume.Value} supports potential colonization");
            }

            if (IsWithinLivableRange(planet.Temperature, -50, 50))
            {
                score += 20;
                reasons.Add($"Temperature at {planet.Temperature} is within a comfortable range for human habitation");
            }

            if (IsWithinLivableRange(planet.Gravity, 0.8, 1.2))
            {
                score += 20;
                reasons.Add($"Gravity level at {planet.Gravity} is conducive to human health and activity");
            }

            if (IsWithinLivableRange(planet.AtmosphericPressure, 0.8, 1.2))
            {
                score += 20;
                reasons.Add($"Atmospheric pressure at {planet.AtmosphericPressure} is within safe limits for human life");
            }

            return (score, reasons);
        }

        private bool IsWithinLivableRange(double? value, double min, double max)
        {
            return value.HasValue && value >= min && value <= max;
        }



    }
}
