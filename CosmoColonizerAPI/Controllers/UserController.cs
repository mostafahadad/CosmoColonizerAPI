using CosmoColonizerAPI.Data.DTOs.User;
using CosmoColonizerAPI.Data.Entities;
using CosmoColonizerAPI.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CosmoColonizerAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet] 
        public async Task<ActionResult<ICollection<User>>> GetAllUsers()
        {
            return Ok(await _userService.GetAllAsync());
        }
        [HttpGet]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            try
            {
                return Ok(await _userService.GetByIdAsync(id));
            }

            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var usernameClaim = User.FindFirst("preferred_username");

                if (userIdClaim == null || usernameClaim == null)
                {
                    return BadRequest("User ID and name claims are required.");
                }

                if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return BadRequest("Invalid user ID claim.");
                }

                string username = usernameClaim.Value;

                int? planetId = DeterminePlanetIdByUsername(username);

                var userDto = new UserDTO
                {
                    Id = userId,
                    Name = username,
                    PlanetId = planetId
                };

                var user = await _userService.AddAsync(userDto);

                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            catch (Exception ex)
            {       
                return StatusCode(500, ex.Message);
            }
        }

        private int? DeterminePlanetIdByUsername(string username)
        {
            if (username.Equals("PlanetAdmin1", StringComparison.OrdinalIgnoreCase)) return 1;
            if (username.Equals("PlanetAdmin2", StringComparison.OrdinalIgnoreCase)) return 2;
            if (username.Equals("PlanetAdmin3", StringComparison.OrdinalIgnoreCase)) return 3;

            return null;
        }
    }
}
                        