using CoreLibrary.Models.Quartz.Listeners;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Impl.AdoJobStore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CoreLibrary;

public static class CoreDiConfiguration
{
    public static void ShedulerQuartzConfigurate(this IServiceCollectionQuartzConfigurator configurator,
        IConfiguration configuration, string nameSheduler)
    {
        configurator.SchedulerId = $"{nameSheduler}_{Guid.NewGuid()}";
        configurator.SchedulerName = $"{nameSheduler}";

        configurator.UseSimpleTypeLoader();
        configurator.UseInMemoryStore();
        configurator.UseTimeZoneConverter();

        configurator.UseDefaultThreadPool(poolOptions => { poolOptions.MaxConcurrency = 100; });

        configurator.UsePersistentStore(storeOptions =>
        {
            storeOptions.PerformSchemaValidation = false; // default
            storeOptions.UseProperties = true;
            storeOptions.RetryInterval = TimeSpan.FromSeconds(15);
            storeOptions.UseMySql(sqlServer =>
            {
                sqlServer.UseDriverDelegate<MySQLDelegate>();
                sqlServer.ConnectionString = configuration.GetConnectionString("QuartzDbContext")!;
                sqlServer.TablePrefix = "QRTZ_";
            });

            storeOptions.UseNewtonsoftJsonSerializer();

            storeOptions.UseClustering(c =>
            {
                c.CheckinMisfireThreshold = TimeSpan.FromSeconds(20);
                c.CheckinInterval = TimeSpan.FromSeconds(10);
            });
        });

        configurator.UseJobAutoInterrupt(options => { options.DefaultMaxRunTime = TimeSpan.FromMinutes(5); });

        configurator.AddSchedulerListener<DefaultSchedulerListener>();
        configurator.AddJobListener<DefaultJobListener>( /*GroupMatcher<JobKey>.GroupEquals(jobKey.Group)*/);
        configurator.AddTriggerListener<DefaultTriggerListener>();
    }

    public static readonly Action<HostBuilderContext, LoggerConfiguration> ConfigurationSerilog = (_, configuration) =>
        configuration.MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.File(new CompactJsonFormatter(), "log.json");

    public static readonly Action<QuartzOptions> OptionsQuartz = quartzOptions =>
    {
        quartzOptions.Scheduling.IgnoreDuplicates = true; // default: false
        quartzOptions.Scheduling.OverWriteExistingData = true; // default: true
    };

    public static readonly Action<SwaggerGenOptions> ConfigurationSwaggerGen = swaggerOption =>
    {
        swaggerOption.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
        swaggerOption.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization Basic",
            Type = SecuritySchemeType.Http,
            Scheme = "basic"
        });
        swaggerOption.AddSignalRSwaggerGen();
        swaggerOption.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Basic"
                    }
                },
                new List<string>()
            }
        });
    };

    public static readonly Action<HttpConnectionDispatcherOptions> ConfigurationHubMap = optionSignalRMap =>
    {
        optionSignalRMap.ApplicationMaxBufferSize = 30000;
        optionSignalRMap.TransportMaxBufferSize = 30000;
        optionSignalRMap.TransportSendTimeout = TimeSpan.FromMinutes(30);
        optionSignalRMap.WebSockets.CloseTimeout = TimeSpan.FromMinutes(30);
        optionSignalRMap.Transports = HttpTransportType.WebSockets;
    };

    public static readonly Action<HubOptions> OptionsHub = hubOptionsDefault =>
    {
        hubOptionsDefault.EnableDetailedErrors = true;
        hubOptionsDefault.KeepAliveInterval = TimeSpan.FromMinutes(30);
        hubOptionsDefault.HandshakeTimeout = TimeSpan.FromMinutes(30);
        hubOptionsDefault.MaximumParallelInvocationsPerClient = 100;
        hubOptionsDefault.ClientTimeoutInterval = TimeSpan.FromMinutes(30);
        hubOptionsDefault.StreamBufferCapacity = 30000;
    };

    public static readonly Action<SessionOptions> SessionOption = options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.Name = ".Web.Session";
        options.Cookie.IsEssential = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    };

    public static readonly Action<CookieAuthenticationOptions> CookieOption = options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

        options.LoginPath = "/Users/Login";
        options.LogoutPath = "/Users/Login";
        options.AccessDeniedPath = "/Home/ErrorCodePage?errorCode=403";
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    };
}