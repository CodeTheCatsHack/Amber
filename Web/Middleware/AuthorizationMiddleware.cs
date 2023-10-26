using System.Net.Http.Headers;
using System.Security.Claims;
using CoreLibrary.EFContext;
using CoreLibrary.Models.ConditionalExpressions.Expansion;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Web.Middleware;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;

    public AuthorizationMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
    {
        _next = next;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await using (var scope = _serviceProvider.CreateAsyncScope())
        {
            var userId = context.Session.GetInt32("UserId");

            if (userId != null)
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ServiceMonitoringContext>();

                var user = await dbContext.Users
                    .IncludesDetailAuthentification()
                    .FirstOrDefaultAsync(x => x.IdUser == userId);

                if (user != null)
                {
                    var identity = context.CreateUserClaimIdentityIssuer(ref dbContext, user);
                    await context.SignInAsync(new ClaimsPrincipal(identity!));
                }
            }
        }

        if (!string.IsNullOrEmpty(context.Request.Headers.Authorization) &&
            AuthenticationHeaderValue.Parse(context.Request.Headers.Authorization).Scheme == "Basic")
        {
            var authenticate = await context.AuthenticateAsync("Basic");
            if (authenticate.Succeeded)
                //Ручная авторизация, так как SignIn не работает в контексте иной авторизации отличной от default.
                context.User = authenticate.Principal;
            else
                context.Response.StatusCode = int.Parse(authenticate.Failure!.Message);
        }

        await _next(context.SetIssuer());
    }
}