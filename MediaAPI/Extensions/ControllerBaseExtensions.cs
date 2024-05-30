using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MediaAPI.Extensions
{
    public static class ControllerExtensions
    {
        public static string GetUsernameFromToken(this ControllerBase controller)
        {
            return controller.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
