using Clave.Expressionify;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoreLibrary.Models.EFModels_Partial;

public static class ConfiguredServiceSystemContext
{
    public static void ConfigurationMySql(this DbContextOptionsBuilder options, IConfiguration configuration)
    {
        options.UseMySql(configuration.GetConnectionString("Amber"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("Amber")),
                x => x.UseNetTopologySuite().EnableRetryOnFailure())
            .UseExpressionify(o => o.WithEvaluationMode(ExpressionEvaluationMode.FullCompatibilityButSlow))
            .UseAllCheckConstraints()
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }

    public static void ConfigurationQuartzMySql(this DbContextOptionsBuilder options, IConfiguration configuration)
    {
        options.UseMySql(configuration.GetConnectionString("QuartzDbContext"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("QuartzDbContext")),
                x => x.UseNetTopologySuite().EnableRetryOnFailure())
            .UseExpressionify(o => o.WithEvaluationMode(ExpressionEvaluationMode.FullCompatibilityButSlow))
            .UseAllCheckConstraints()
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }
}