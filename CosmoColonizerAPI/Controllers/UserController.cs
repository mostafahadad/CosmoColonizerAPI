using CosmoColonizerAPI.Data.Entities;
using CosmoColonizerAPI.Services.Users;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CosmoColonizerAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
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
                var nameClaim = User.FindFirst(ClaimTypes.Name);

                if (userIdClaim == null || nameClaim == null)
                {
                    return BadRequest("User ID and name claims are required.");
                }

                if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return BadRequest("Invalid user ID claim.");
                }

                string name = nameClaim.Value;

                var user = await _userService.AddAsync(userId, name);

                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                // Handle other exceptions appropriately
                return StatusCode(500, ex.Message);
            }
        }
    }
}
                        