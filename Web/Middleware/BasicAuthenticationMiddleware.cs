using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using CoreLibrary.EFContext;
using CoreLibrary.Models.ConditionalExpressions.Expansion;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Web.Middleware;

public class BasicAuthenticationMiddleware : AuthenticationHandler<BasicAuthenticationOptions>
{
    private readonly ServiceMonitoringContext _context;

    public BasicAuthenticationMiddleware(
        IOptionsMonitor<BasicAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock, ServiceMonitoringContext
            context)
        : base(options, logger, encoder, clock)
    {
        _context = context;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers.Authorization);

            if (!string.IsNullOrEmpty(authHeader.Parameter))
            {
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                var login = credentials[0];
                var password = credentials[1];

                var user = await _context.Users.FindUser(_context, login, password, true);

                if (user != null)
                {
                    var identity = Context.SetIssuer().CreateUserClaimIdentityIssuer(user);

                    if (identity != null)
                    {
                        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), "Basic");
                        return AuthenticateResult.Success(ticket);
                    }
                }
            }

            return AuthenticateResult.Fail("403");
        }
        catch (Exception ex)
        {
            return AuthenticateResult.Fail(ex.Message);
        }
    }
}

public class BasicAuthenticationOptions : AuthenticationSchemeOptions
{
}