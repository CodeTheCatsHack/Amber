using System.Security.Claims;
using CoreLibrary.EFContext;
using Microsoft.AspNetCore.SignalR;
using Web.Controllers.Abstraction;

namespace CoreLibrary.Models.ConditionalExpressions.Expansion;

public static class ExpansionControllerRelationApiDevice
{
    public enum ESendClaimType
    {
        User,
        AccessGroup,
        Organization
    }

    /// <summary>
    ///     Метод уведомляет пользователей о каких либо действиях на авторизованных девайсах.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="text"></param>
    /// <param name="title"></param>
    /// <param name="personOrGroup"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static async Task NotifyRealApiDeviceUser<THub>(this AbstractController<THub, ServiceMonitoringContext
        > controller, string text,
        string title = "Уведомление", ESendClaimType personOrGroup = ESendClaimType.User) where THub : Hub
    {
        var claim = personOrGroup switch
        {
            ESendClaimType.User => controller.User.GetClaimIssuer(ClaimTypes.Sid),
            ESendClaimType.AccessGroup => controller.User.GetClaimIssuer(ClaimTypes.GroupSid),
            ESendClaimType.Organization => controller.User.GetClaimIssuer(ClaimTypes.System),
            _ => throw new ArgumentOutOfRangeException(nameof(personOrGroup), personOrGroup, null)
        };

        if (claim != null)
            switch (personOrGroup)
            {
                case ESendClaimType.User:
                    await controller.RealApiContext.Clients.User(claim.Value).SendAsync("Notify", $"{title}:{text}");
                    break;
                case ESendClaimType.AccessGroup:
                    await controller.RealApiContext.Clients.Group($"Access:{claim.Value}")
                        .SendAsync("Notify", $"{title}:{text}");
                    break;
                case ESendClaimType.Organization:
                    await controller.RealApiContext.Clients.Groups($"Group:{claim.Value}")
                        .SendAsync("Notify", $"{title}:{text}");
                    break;
            }
    }
}