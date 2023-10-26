using System.Security.Claims;
using System.Text.Json;
using CoreLibrary.EFContext;
using CoreLibrary.Models.ConditionalExpressions.Expansion;
using CoreLibrary.Models.ConditionalExpressions.Expression;
using CoreLibrary.Models.EFModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Web.Controllers.Abstraction;
using Web.Models;
using Web.SignalRHub;

namespace Web.Controllers;

public class MapController : AbstractController<HubMaps, ServiceMonitoringContext>, IControllerBaseAction<Marker>
{
    public MapController(ServiceMonitoringContext dataBaseContext, IHubContext<HubMaps> realApiContext) : base(
        dataBaseContext, realApiContext)
    {
    }

    public IActionResult Edit()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Map()
    {
        var markers = await DataBaseContext.Markers
            .Where(x => x.Map == User.GetClaimIssuer<int>(ClaimTypes.Rsa)).ToListAsync();
        if (markers.Count != 0)
            await RealApiContext.Clients.All.SendAsync("UpdateMarkers",
                JsonSerializer.Serialize(markers));
        //await RealApiContext.Clients.All.SendAsync("UpdateSatelline","");

        return View();
    }
    //
    // public async Task<IActionResult> MarkerTableData()
    // {
    //     var systemObjects = await DataBaseContext.Markers.IncludePartialFullInfo()
    //         .ConstrainOrganization(User.GetClaimIssuer(ClaimTypes.System)).Take(10).ToListAsync();
    //     var dataTable = new ModelDataTable<Marker>(systemObjects);
    //     return View(dataTable);
    // }
    //
    // [HttpPost]
    // public async Task<IActionResult> MarkerTableData([Bind("Models,Search,Skip,Take")] ModelDataTable<Marker> MarkerObject)
    // {
    //     var query = DataBaseContext.Markers.IncludePartialFullInfo().ConstrainOrganization(User.GetClaimIssuer(ClaimTypes.System));
    //     MarkerObject.Models = await ((MarkerObject.Skip, MarkerObject.Take) switch
    //     {
    //         (0, 0) => query,
    //         ( > 0, > 0) => query.Skip(MarkerObject.Skip).Take(MarkerObject.Take),
    //         (0, > 0) => query.Take(MarkerObject.Take),
    //         ( > 0, 0) => query.Skip(MarkerObject.Skip)
    //     }).ToListAsync();
    //
    //     if (!string.IsNullOrEmpty(MarkerObject.Search))
    //         MarkerObject.Models = MarkerObject.Models.Where(user => user.ReflectionPropertiesSearch(MarkerObject.Search)).ToList();
    //
    //     return View(MarkerObject);
    // }
    //
    // public async Task<IActionResult> Create()
    // {
    //     ViewData["SystemObjects"] = await DataBaseContext.Markers.IncludePartialFullInfo().ConstrainOrganization(User.GetClaimIssuer(ClaimTypes.System)).ToListAsync();
    //     return View();
    // }
    //
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Create([Bind("IdMarker,SystemObjectId,Latitude,Longitude")] Marker Marker)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         DataBaseContext.Add(Marker);
    //         await DataBaseContext.SaveChangesAsync();
    //         return RedirectToAction(nameof(MarkerTableData));
    //     }
    //     
    //     ViewData["SystemObjects"] = await DataBaseContext.Markers.IncludePartialFullInfo().ConstrainOrganization(User.GetClaimIssuer(ClaimTypes.System)).ToListAsync();
    //     return View(Marker);
    // }
    //
    // public async Task<IActionResult> Edit(int? id)
    // {
    //     if (id == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     var Marker = await DataBaseContext.Markers.FirstOrDefaultAsync(x => x.IdMarker == id);
    //     if (Marker == null)
    //     {
    //         return NotFound();
    //     }
    //     
    //     ViewData["SystemObjects"] = await DataBaseContext.Markers.IncludePartialFullInfo().ConstrainOrganization(User.GetClaimIssuer(ClaimTypes.System)).ToListAsync();
    //     return View(Marker);
    // }
    //
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Edit(int id, [Bind("IdMarker,SystemObjectId,Latitude,Longitude")] Marker Marker)
    // {
    //     if (id != Marker.IdMarker)
    //     {
    //         return NotFound();
    //     }
    //
    //     if (ModelState.IsValid)
    //     {
    //         try
    //         {
    //             DataBaseContext.Update(Marker);
    //             await DataBaseContext.SaveChangesAsync();
    //         }
    //         catch (DbUpdateConcurrencyException)
    //         {
    //             if (!MarkerExists(Marker.IdMarker))
    //             {
    //                 return NotFound();
    //             }
    //             else
    //             {
    //                 throw;
    //             }
    //         }
    //         return RedirectToAction(nameof(MarkerTableData));
    //     }
    //     
    //     ViewData["SystemObjects"] = await DataBaseContext.Markers.IncludePartialFullInfo().ConstrainOrganization(User.GetClaimIssuer(ClaimTypes.System)).ToListAsync();
    //     return View(Marker);
    // }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var Marker = await DataBaseContext.Markers
            .FirstOrDefaultAsync(m => m.IdMarker == id);
        if (Marker == null) return NotFound();

        return View(Marker);
    }

    // [HttpPost, ActionName("Delete")]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> DeleteConfirmed(int id)
    // {
    //     var Marker = await DataBaseContext.Markers.FirstOrDefaultAsync(x => x.IdMarker == id);
    //     if (Marker != null)
    //     {
    //         DataBaseContext.Markers.Remove(Marker);
    //     }
    //
    //     await DataBaseContext.SaveChangesAsync();
    //     return RedirectToAction(nameof(MarkerTableData));
    // }

    private bool MarkerExists(int id)
    {
        return DataBaseContext.Markers.Any(e => e.IdMarker == id);
    }

    /// <summary>
    ///     Метод отображения таблицы пользовательских аккаунтов.
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> MapTableData()
    {
        var maps = await DataBaseContext.Maps.Take(10).ToListAsync();
        var dataTable = new ModelDataTable<Map>(maps);
        return View(dataTable);
    }

    /// <summary>
    ///     Метод отображения таблицы пользовательских аккаунтов.
    /// </summary>
    /// <param name="tempMaps"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> MapTableData([Bind("Models,Search,Skip,Take")] ModelDataTable<Map> tempMaps)
    {
        var query = DataBaseContext.Maps.ToList();

        tempMaps.Models = (tempMaps.Skip, tempMaps.Take) switch
        {
            (0, 0) => query,
            (> 0, > 0) => query.Skip(tempMaps.Skip).Take(tempMaps.Take).ToList(),
            (0, > 0) => query.Take(tempMaps.Take).ToList(),
            (> 0, 0) => query.Skip(tempMaps.Skip).ToList()
        };

        if (!string.IsNullOrEmpty(tempMaps.Search))
            tempMaps.Models = tempMaps.Models.Where(user => user.ReflectionPropertiesSearch(tempMaps.Search))
                .ToList();

        return View(tempMaps);
    }
}