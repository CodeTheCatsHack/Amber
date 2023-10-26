using CoreLibrary.EFContext;
using CoreLibrary.Models.EFModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SatLib;
using Web.Controllers.Abstraction;
using Web.SignalRHub;

namespace Web.Controllers;

public class InformationSystemController : AbstractController<HubMaps, ServiceMonitoringContext>
{
    public readonly SatelliteApi SatelliteApi;

    public InformationSystemController(ServiceMonitoringContext context, IHubContext<HubMaps> realApiContext,
        SatelliteApi api) :
        base(context, realApiContext)
    {
        SatelliteApi = api;
    }

    [Authorize]
    public async Task<IActionResult> Monitoring()
    {
        var model = new Map
        {
            Name = "Карта по умолчанию",
            Markers = new List<Marker>
            {
                new()
                {
                    Latitude = 64,
                    Longitude = 34,
                    Name = "Арх",
                    Radius = 20
                }
            }
        };
        model.Satellites = SatelliteApi.SearchSolution();
        return View(model);
    }
}