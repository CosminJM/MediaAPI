using HotChocolate.Resolvers;
using Media.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MediaAPI.Middlewares
{
    public class UserMiddleware
    {

        public const string USER_CONTEXT_DATA_KEY = "User";

        private readonly FieldDelegate _next;

        public UserMiddleware(FieldDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(IMiddlewareContext context)
        {
            if (context.ContextData.TryGetValue("ClaimsPrincipal", out object rawClaimsPrincipal) &&
                rawClaimsPrincipal is ClaimsPrincipal claimsPrincipal)
            {

                User user = new User()
                {
                    Username = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier),
                };

                context.ContextData.Add(USER_CONTEXT_DATA_KEY, user);
            }

            await _next(context);
        }
    }
}
