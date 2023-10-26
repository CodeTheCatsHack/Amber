using System.Data.Entity;
using System.Security.Claims;
using CoreLibrary.EFContext;
using CoreLibrary.Models.ConditionalExpressions.Expansion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Web.Controllers.Abstraction;
using Web.SignalRHub;

namespace Web.Controllers;

public class InformationSystemController : AbstractController<HubMaps, ServiceMonitoringContext>
{
    public InformationSystemController(ServiceMonitoringContext context, IHubContext<HubMaps> realApiContext) :
        base(context, realApiContext)
    {
    }

    [Authorize]
    public async Task<IActionResult> Monitoring()
    {
        if (int.TryParse(User.GetClaimIssuer(ClaimTypes.System)?.Value, out var isId))
            if (isId == 1)
            {
                var model = DataBaseContext.Maps.AsNoTracking().ToList();
                if (model.Count == 0) model = null;
                return View(model);
            }

        return View();
    }
}