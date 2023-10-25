using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Models.Abstraction;

public abstract class AbstractDbHub<TDataBase, THub> where TDataBase : DbContext where THub : Hub
{
    protected readonly TDataBase? ContextDataBase;
    protected readonly IHubContext<THub>? ContextHub;

    protected AbstractDbHub(TDataBase? gameSystemContextDataBase, IHubContext<THub> contextHub)
    {
        ContextDataBase = gameSystemContextDataBase;
        ContextHub = contextHub;
    }
}