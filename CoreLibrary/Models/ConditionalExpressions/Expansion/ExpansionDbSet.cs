using CoreLibrary.EFContext;
using CoreLibrary.Models.EFModels;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Models.ConditionalExpressions.Expansion;

public static class ExpansionDbSet
{
    public static async Task<User?> FindUser(this DbSet<User> users, ServiceMonitoringContext context, User user)
    {
        return (await users.IncludesDetailAuthentification()
                .Where(x => x.Login == user.Login).ToListAsync())
            .FirstOrDefault(x => x.IsValidPassword(context, user.Password));
    }

    public static async Task<User?> FindUser(this DbSet<User> users, ServiceMonitoringContext context, string login,
        string password, bool detail = false)
    {
        return (await users.IncludesDetailAuthentification()
                .Where(x => x.Login == login).ToListAsync())
            .FirstOrDefault(x => x.IsValidPassword(context, password));
    }

    public static IQueryable<User> IncludesDetailAuthentification(this DbSet<User> users)
    {
        return users.Include(x => x.InformationUser);
    }

    public static IQueryable<User> IncludePartialFullInfo(this DbSet<User> users)
    {
        return users
            .Include(x => x.InformationUser);
    }
}