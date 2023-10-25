using CoreLibrary.Context;
using CoreLibrary.EFContext;
using CoreLibrary.Models.EFModels_Partial;
using CoreLibrary.Models.Quartz.PostProcessor;
using CoreLibrary.Models.Quartz.PreProcessors;
using CoreLibrary.Models.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Serilog;
using static CoreLibrary.CoreDiConfiguration;

namespace CoreLibrary;

public static class CoreDi
{
    public static IServiceCollection AddCoreDi(this IServiceCollection services, IConfiguration configuration,
        string nameSheduler)
    {
        return services.AddLoggers()
            .AddDataBaseContext(configuration)
            .AddQuartzIntegrated(configuration, nameSheduler)
            .AddSignalRIntegrated()
            .AddSession(SessionOption);
    }

    public static IServiceCollection AddLoggers(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
    }

    public static IServiceCollection AddDataBaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext<ServiceMonitoringContext>(o => o.ConfigurationMySql(configuration))
            .AddDbContext<QuartzDbContext>(x => x.ConfigurationQuartzMySql(configuration));
    }

    public static IServiceCollection AddQuartzIntegrated(this IServiceCollection services, IConfiguration configuration,
        string nameSheduler)
    {
        return services.Configure(OptionsQuartz)
            .AddQuartz(configurator =>
            {
                configurator.ShedulerQuartzConfigurate(configuration, nameSheduler);
                configurator.AddCallendarsQuartz();
                configurator.AddJobsQuartz();
                configurator.AddTriggersQuartz();
            })
            .AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; })
            .AddQuartzSingletons();
    }

    public static IServiceCollection AddSignalRIntegrated(this IServiceCollection services)
    {
        services
            .AddSingleton<IUserIdProvider, CustomUserIdProvider>()
            .AddSignalR(OptionsHub);

        return services;
    }
}