
namespace MediaAPI.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Returns a token string
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<string> AuthenticateAsync(string username, string password);
        /// <summary>
        /// Generate new authentication token for refresh token use case
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<string> AuthenticateAsync(string username);

        /// <summary>
        /// Generates a random refresh token to apply as cookie for authentication token
        /// </summary>
        /// <returns></returns>
        string GenerateRefreshToken();
        Task RegisterAsync(string username, string email, string password);
    }
}