
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
        Task RegisterAsync(string username, string email, string password);
    }
}