using System.Security.Claims;
using System.Text.Json;
using CoreLibrary.EFContext;
using CoreLibrary.Models.EFModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Web.Controllers.Abstraction;

namespace CoreLibrary.Models.ConditionalExpressions.Expansion;

public static class ExpansionClaimsPrincipal
{
    private static string? _userIssuer;

    /// <summary>
    ///     Создание авторизационных данных для определённой схемы, в виде идентифицированного набора клеймов.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="user"></param>
    public static ClaimsIdentity? CreateUserClaimIdentityIssuer(this HttpContext context,
        ref ServiceMonitoringContext dbContext, User user,
        string shema = CookieAuthenticationDefaults.AuthenticationScheme)
    {
        if (_userIssuer != null)
        {
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Sid, user.IdUser.ToString(), nameof(Int32), _userIssuer),
                new(ClaimTypes.UserData, JsonSerializer.Serialize(user), nameof(User), _userIssuer),
                new(ClaimTypes.System,
                    user.GroundStateAdministrator.ToString(), nameof(Int32), _userIssuer)
            }, shema);

            var map = dbContext.Maps.AsNoTracking().ToList().FirstOrDefault(x => x.User == user.IdUser);

            if (map != null)
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Rsa, map.IdMap.ToString(), nameof(Int32), _userIssuer));

            return claimsIdentity;
        }

        return null;
    }

    /// <summary>
    ///     Задаёт поставщика аккаунта для данного запуска.
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="issuer">Поставщик</param>
    public static HttpContext SetIssuer(this HttpContext httpContext, string issuer = "NewCustom")
    {
        _userIssuer = issuer;
        return httpContext;
    }

    #region Методы сверки даннных внутри клеймов

    /// <summary>
    ///     Метод определяет поставщика данных данного запуска для обхода возможных проблем с куки.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static bool IsInIssuer(this ClaimsPrincipal user)
    {
        return user.HasClaim(x => x.Issuer == _userIssuer);
    }


    /// <summary>
    ///     Метод проверяет авторизационные данные пользователя в веб приложении.
    /// </summary>
    /// <param name="controller">Контроллер из которого происходит проверка.</param>
    /// <param name="contextUse">Использовать ли запрос к базе данных или просто убедиться в наличии данных.</param>
    /// <returns></returns>
    public static async Task<bool> IsAuthUser<THub>(this AbstractController<THub, ServiceMonitoringContext> controller,
        bool contextUse = false) where THub : Hub
    {
        try
        {
            var idUser = controller.User.GetClaimIssuer(ClaimTypes.Sid);
            var userData = controller.User.GetClaimIssuer<User>(ClaimTypes.UserData);

            if (idUser is null || userData is null) return false;
            if (!contextUse) return true;

            return await controller.DataBaseContext.Users.AnyAsync(x => userData.IsValidUser(x));
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    ///     Метод проверяет причастность к организации авторизованного пользователя.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="idOrganization"></param>
    /// <returns></returns>
    public static bool IsInOrganization(this ClaimsPrincipal user, int idOrganization)
    {
        return user.HasClaim(x => x.Type == ClaimTypes.System && x.Value == idOrganization.ToString());
    }

    #endregion

    #region Методы извлечения клеймов информации

    public static Dictionary<string, List<string>>? GetClaimIssuer(this ClaimsPrincipal claimsPrincipal)
    {
        var claims = claimsPrincipal.Claims.Where(x => x.Issuer == _userIssuer).ToList();
        return claims.Count != 0
            ? claims.ToDictionary(x => x.Type, x => claims.Where(a => a.Type == x.Type).Select(b => b.Value).ToList())
            : null;
    }

    public static Claim? GetClaimIssuer(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        var claim = claimsPrincipal.Claims.FirstOrDefault(x => x.Issuer == _userIssuer && x.Type == claimType);
        return claim ?? null;
    }

    public static T? GetClaimIssuer<T>(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        var claim = claimsPrincipal.Claims.FirstOrDefault(x => x.Issuer == _userIssuer && x.Type == claimType);
        if (claim != null)
            try
            {
                return JsonSerializer.Deserialize<T>(claim.Value);
            }
            catch (Exception)
            {
                //ignore
            }

        return default;
    }

    #endregion
}