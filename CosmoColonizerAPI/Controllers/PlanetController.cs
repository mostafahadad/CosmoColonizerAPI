using CosmoColonizerAPI.Data.Entities;
using CosmoColonizerAPI.Services.Planets;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<ICollection<Planet>>> GetAllPlanets()
        {
            return Ok(await _planetsService.GetAllAsync());
        }

        [HttpGet]
        public async Task<ActionResult<Planet>> GetPlanet(int id)
        {
            try
            {
                return Ok(await _planetsService.GetByIdAsync(id));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
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
                // Log the exception details here
                return StatusCode(500, $"An error occurred while updating the post: {ex.Message}");
            }
        }

    }
}
