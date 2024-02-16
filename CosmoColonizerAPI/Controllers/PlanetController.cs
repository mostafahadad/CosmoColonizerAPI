using CosmoColonizerAPI.Data.Entities;
using CosmoColonizerAPI.Services.Planets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;
using System.Security.Claims;

namespace CosmoColonizerAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PlanetController : ControllerBase
    {
        private readonly IPlanetsService _planetsService;
        public PlanetController(IPlanetsService planetsService)
        {
            _planetsService = planetsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Planet>>> GetPlanets()
        {
            try
            {
                var username = User.FindFirst("preferred_username")?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized($"Unable to determine the user's identity. {username}");
                }

                if (User.IsInRole("Super Admin")){
                    return Ok(await _planetsService.GetAllAsync());
                }


                if (User.IsInRole("Planet Admin")){
                    int assignedPlanetId = GetAssignedPlanetIdForAdmin(username); 
                    var planet = await _planetsService.GetByIdAsync(assignedPlanetId);
                    
                    return Ok(new List<Planet> { planet });
                }

                if(User.IsInRole("Viewer"))
                {
                    if (username.Equals("viewer1", StringComparison.OrdinalIgnoreCase))
                    {
                        var planet = await _planetsService.GetByIdAsync(1);
                        return Ok(new List<Planet> { planet });
                    }
                    if(username.Equals("viewer2", StringComparison.OrdinalIgnoreCase))
                    {
                        var planets = await _planetsService.GetByIdsAsync(new List<int> { 1, 3 });
                        return Ok(planets);
                    }
                }

                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        private int GetAssignedPlanetIdForAdmin(string username)
        {
            if (username.Equals("PlanetAdmin1", StringComparison.OrdinalIgnoreCase))
            {
                return 1;
            }
            else if (username.Equals("PlanetAdmin2", StringComparison.OrdinalIgnoreCase))
            {
                return 2;
            }
            else if (username.Equals("PlanetAdmin3", StringComparison.OrdinalIgnoreCase))
            {
                return 3;
            }

            throw new ArgumentException($"Invalid or unrecognized PlanetAdmin username: {username}");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Planet>> UpdatePlanet(int id, Planet planet)
        {
            if(id != planet.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the provided data.");
            }

            try
            {
                var updatedPlanet = await _planetsService.UpdateAsync(planet);
                return Ok(updatedPlanet);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the post: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult> SuggestBestPlanet()
        {
            try
            {
                var (BestPlanet, Explanation) = await _planetsService.SuggestBestPlanetForColonizationAsync();

                return Ok(new { BestPlanet, Explanation });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while suggesting the best planet: {ex.Message}");
            }
        }



    }
}
