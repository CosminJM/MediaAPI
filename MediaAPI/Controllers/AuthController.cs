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
        //private static readonly List<RefreshToken> refreshTokens = new List<RefreshToken>();
        //private const string REFRESH_TOKEN_COOKIE_KEY = "refreshToken";
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

            //var refreshToken = _authService.GenerateRefreshToken();
            //refreshTokens.Add(new RefreshToken() { Token = refreshToken, Username = model.Username, UserId = user.Id });

            //Response.Cookies.Append(REFRESH_TOKEN_COOKIE_KEY, refreshToken, new CookieOptions
            //{
            //    HttpOnly = true,
            //    Secure = true,
            //    SameSite = SameSiteMode.Strict,
            //    MaxAge = TimeSpan.FromDays(1)
            //});


            return Ok(new { token , user.Id, user.Username});
        }

        //[HttpPost("refresh")]
        //public IActionResult Refresh()
        //{
        //    var refreshToken = Request.Cookies[REFRESH_TOKEN_COOKIE_KEY];
        //    var storedRefreshToken = refreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);

        //    if (storedRefreshToken == null)
        //    {
        //        return Unauthorized();
        //    }

        //    var newAccessToken = _authService.AuthenticateAsync(storedRefreshToken.Username);
        //    var newRefreshToken = _authService.GenerateRefreshToken();

        //    refreshTokens.Remove(storedRefreshToken);
        //    refreshTokens.Add(new RefreshToken { Token = newRefreshToken, Username = storedRefreshToken.Username });

        //    Response.Cookies.Append(REFRESH_TOKEN_COOKIE_KEY, newRefreshToken, new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Secure = true,
        //        SameSite = SameSiteMode.Strict,
        //        MaxAge = TimeSpan.FromDays(1)
        //    });

        //    return Ok(new { token = newAccessToken, storedRefreshToken.Username, storedRefreshToken.UserId });
        //}

        //[HttpPost("logout")]
        //public IActionResult Logout()
        //{
        //    var refreshToken = Request.Cookies[REFRESH_TOKEN_COOKIE_KEY];
        //    var storedRefreshToken = refreshTokens.FirstOrDefault(rt => rt.Token == refreshToken);

        //    if (storedRefreshToken != null)
        //    {
        //        refreshTokens.Remove(storedRefreshToken);
        //    }

        //    Response.Cookies.Delete(REFRESH_TOKEN_COOKIE_KEY);

        //    return Ok();
        //}
    }
}
