using System.Security.Claims;
using CoreLibrary;
using Microsoft.AspNetCore.Authentication.Cookies;
using SatLib;
using Serilog;
using SGPdotNET.CoordinateSystem;
using Web.Middleware;
using Web.SignalRHub;
using static CoreLibrary.CoreDiConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(ConfigurationSerilog);

builder.Services.AddCoreDi(builder.Configuration, "ShedulerMonitoring")
    .AddSwaggerGen(ConfigurationSwaggerGen);

builder.Services.AddSingleton<Configurator>();
builder.Services.AddSingleton<SatelliteApi>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddScheme<BasicAuthenticationOptions, BasicAuthenticationMiddleware>("Basic", options => { })
    .AddCookie(CookieOption);

builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("OrganizationAdministrationPolicy", p => p.RequireClaim(ClaimTypes.System));
    x.AddPolicy("OrganizationPolicy", p => p.RequireClaim(ClaimTypes.Sid));
});

builder.Services.AddCors();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials())
        .UseSwagger()
        .UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/Home/ErrorCodePage?errorCode={0}");

app.UseHttpsRedirection();
app.UseSession();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.UseMiddleware<AuthorizationMiddleware>(app.Services);

app.UseMvcWithDefaultRoute();

app.MapHub<HubMaps>("/api", ConfigurationHubMap);

app.Run();