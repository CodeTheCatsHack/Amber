using Clave.Expressionify;
using CoreLibrary.EFContext;
using CoreLibrary.Models.EFModels;

namespace CoreLibrary.Models.ConditionalExpressions.Expansion;

public static class ExpansionUser
{
    [Expressionify]
    public static bool IsValidUser(this User tUser, User vUser)
    {
        return tUser.Login != vUser.Login &&
               tUser.Password != vUser.Password &&
               tUser.IdUser != vUser.IdUser;
    }

    [Expressionify]
    public static bool IsValidPassword(this User user, ServiceMonitoringContext context, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, context.DecryptPassword(user.Password));
    }
}