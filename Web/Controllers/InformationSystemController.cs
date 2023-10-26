using CoreLibrary.EFContext;
using CoreLibrary.Models.EFModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SatLib;
using SatLib.JsonModel;
using SGPdotNET.CoordinateSystem;
using Web.Controllers.Abstraction;
using Web.Models;
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
            Markers = DataBaseContext.Markers.ToList(),
            Satellites = DataBaseContext.Satellites.ToList()
        };

        var listSetellinesActive = new AboveJson();

        var listSatellines = new List<Satellite>();

        var listSatellinesA = new List<Satellite>();

        foreach (var marker in model.Markers)
        {
            var geo = new GeodeticCoordinate(marker.Latitude, marker.Longitude, 14);
            listSetellinesActive = SatelliteApi.RequestNearestSatellites(geo, 30, SatelliteApi.SatelliteCategory.All);
            foreach (var satelliteJson in listSetellinesActive.Above)
                if (DataBaseContext.Satellites
                        .FirstOrDefault(x => x.IdSatellite == satelliteJson.SatId) != null)
                    listSatellines.Add(DataBaseContext.Satellites
                        .FirstOrDefault(x => x.IdSatellite == satelliteJson.SatId));

            var set = await SatelliteApi.SearchSolutionAsync(geo, SatelliteApi.SatelliteCategory.All);
            foreach (var satelliteJson in set)
                if (DataBaseContext.Satellites
                        .FirstOrDefault(x => x.IdSatellite == satelliteJson.Norad) != null)
                    listSatellinesA.Add(DataBaseContext.Satellites
                        .FirstOrDefault(x => x.IdSatellite == satelliteJson.Norad));
        }

        var dictionary =
            new Dictionary<int, (List<Satellite>, List<Satellite>)>();

        foreach (var marker in model.Markers) dictionary.Add(marker.IdMarker, (listSatellines, listSatellinesA));

        return View(new ModelDataNewTable<Satellite>
        {
            Item1 = model,
            Item2 = dictionary
        });
    }
}