using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Abstraction;

public abstract class AbstractController<TController, THub, TDbContext> : Controller
    where THub : Hub where TDbContext : DbContext
{
    public readonly TDbContext DataBaseContext;
    public readonly IHubContext<THub> RealApiContext;
    public readonly ILogger<TController> LoggerController;

    protected AbstractController(TDbContext dataBaseContext, IHubContext<THub> realApiContext,
        ILogger<TController> loggerController)
    {
        DataBaseContext = dataBaseContext;
        RealApiContext = realApiContext;
        LoggerController = loggerController;
    }
}

public abstract class AbstractController<TController> : Controller
{
    public readonly ILogger<TController> LoggerController;

    protected AbstractController(ILogger<TController> loggerController)
    {
        LoggerController = loggerController;
    }
}

public abstract class AbstractController<THub, TDbContext> : Controller where THub : Hub where TDbContext : DbContext
{
    public readonly TDbContext DataBaseContext;
    public readonly IHubContext<THub> RealApiContext;

    protected AbstractController(TDbContext dataBaseContext, IHubContext<THub> realApiContext)
    {
        DataBaseContext = dataBaseContext;
        RealApiContext = realApiContext;
    }
}