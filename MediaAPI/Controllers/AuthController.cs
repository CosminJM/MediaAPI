using Media.DataAccess.Repository;
using MediaAPI.Models;
using MediaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediaAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public AuthController(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegistrationDto model)
        {
            try
            {
                await _authService.RegisterAsync(model.Username, model.Email, model.Password);
                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto model)
        {
            var token = await _authService.AuthenticateAsync(model.Username, model.Password);

            if (token == null)
            {
                return Unauthorized("Invalid username or password");
            }

            var user = await _userRepository.GetUserByUsernameAsync(model.Username);

            return Ok(new { token , user.Id, user.Username});
        }
    }
}
