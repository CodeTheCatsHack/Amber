using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;

namespace CoreLibrary.Models.Quartz.PostProcessor;

public static class QuartzPostProcessor
{
    public static IServiceCollection AddQuartzSingletons(this IServiceCollection service)
    {
        return service.AddSingleton<ISchedulerFactory, StdSchedulerFactory>()
            .AddSingleton(async provider => await provider.GetRequiredService<ISchedulerFactory>().GetScheduler());
    }
}