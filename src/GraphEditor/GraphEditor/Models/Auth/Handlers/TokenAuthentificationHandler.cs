using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace GraphEditor.Models.Auth.Handlers
{
    public class TokenAuthentificationHandler : IAuthenticationHandler
    {
        private HttpContext? context;

        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            this.context = context;
            return Task.CompletedTask;
        }

        public async Task<AuthenticateResult> AuthenticateAsync()
        {
            var token = await context.GetTokenAsync(StringConstants.TokenAuthenticationDefaultScheme,
                                                    StringConstants.AuthTokenName);            
            var ticket = new AuthenticationTicket(ClaimsPrincipal.Current, StringConstants.TokenAuthenticationDefaultScheme);
            return  AuthenticateResult.Success(ticket);
        }
            

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            return Task.CompletedTask;
        }
    }
}
