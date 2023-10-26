using CoreLibrary.EFContext;
using CoreLibrary.Models.EFModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SatLib;
using SGPdotNET.CoordinateSystem;
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
            },
            Satellites = new List<Satellite>()
        };
        Coordinate arh = new GeodeticCoordinate(1.58352, 11.52378, 14);
        foreach (var set in await SatelliteApi.SearchSolutionAsync(arh, SatelliteApi.SatelliteCategory.All))
            model.Satellites.Add(await DataBaseContext.Satellites.FindAsync(set.Norad));
        return View(model);
    }
}