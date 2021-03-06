using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace web_app.Infrastructure
{
    public class AccessTokenAuthenticationEvents : CookieAuthenticationEvents
    {
        /// <inheritdoc />
        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var hasValidExpirationDate = DateTimeOffset.TryParse(context.Properties.GetTokenValue("expires_at"), out var expiresAt);

            if (!hasValidExpirationDate || expiresAt.UtcDateTime < DateTime.UtcNow - TimeSpan.FromSeconds(30))
            {
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync("Cookies");
            }
        }
    }
}