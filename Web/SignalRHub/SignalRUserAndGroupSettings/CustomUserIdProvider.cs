using System.Security.Claims;
using CoreLibrary.Models.ConditionalExpressions.Expansion;
using Microsoft.AspNetCore.SignalR;

namespace Web.SignalRHub.SignalRUserAndGroupSettings;

public class CustomUserIdProvider : IUserIdProvider
{
    /// <summary>
    ///     Установка уникального пользовательского идентификатора для корректного обращения к устройствам из веб сайта.
    /// </summary>
    /// <param name="connection"></param>
    /// <returns></returns>
    public virtual string? GetUserId(HubConnectionContext connection)
    {
        var httpContext = connection.GetHttpContext();
        var user = httpContext!.User.GetClaimIssuer(ClaimTypes.Sid);
        return user?.Value;
    }
}