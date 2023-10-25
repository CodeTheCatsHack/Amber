using CoreLibrary.EFContext;
using CoreLibrary.Models.EFModels;
using Microsoft.AspNetCore.SignalR;
using Web.Controllers.Abstraction;
using Web.SignalRHub;

namespace Web.Controllers;

public class InformationUsersController : AbstractController<HubMaps, ServiceMonitoringContext>,
    IControllerBaseAction<InformationUser>
{
    public InformationUsersController(ServiceMonitoringContext dataBaseContext, IHubContext<HubMaps> realApiContext) :
        base(dataBaseContext, realApiContext)
    {
    }
}