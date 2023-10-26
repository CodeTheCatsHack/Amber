using CoreLibrary.EFContext;
using CoreLibrary.Models.EFModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Web.Controllers.Abstraction;
using Web.Models;
using Web.SignalRHub;

namespace Web.Controllers;

public class SatelliteController : AbstractController<HubMaps, ServiceMonitoringContext>
{
    public SatelliteController(ServiceMonitoringContext dataBaseContext, IHubContext<HubMaps> realApiContext) : base(
        dataBaseContext, realApiContext)
    {
    }

    // Метод для отображения данных таблицы
    public async Task<IActionResult> SatelliteTableData()
    {
        var satellites = new ModelDataTable<Satellite>(await DataBaseContext.Satellites.ToListAsync());
        return View(satellites);
    }

    // // Метод для создания записи
    // public IActionResult Create()
    // {
    //     return View();
    // }
    //
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Create(Satellite satellite)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         DataBaseContext.Add(satellite);
    //         await DataBaseContext.SaveChangesAsync();
    //         return RedirectToAction("SatelliteTableData");
    //     }
    //     
    //     return View(satellite);
    // }
    //
    // // Метод для редактирования записи
    // public async Task<IActionResult> Edit(int? id)
    // {
    //     if (id == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     var satellite = await DataBaseContext.Satellites.FindAsync(id);
    //     if (satellite == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     return View(satellite);
    // }
    //
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Edit(int id, Satellite satellite)
    // {
    //     if (id != satellite.IdSatellite)
    //     {
    //         return NotFound();
    //     }
    //
    //     if (ModelState.IsValid)
    //     {
    //         try
    //         {
    //             DataBaseContext.Update(satellite);
    //             await DataBaseContext.SaveChangesAsync();
    //         }
    //         catch (DbUpdateConcurrencyException)
    //         {
    //             if (!SatelliteExists(satellite.IdSatellite))
    //             {
    //                 return NotFound();
    //             }
    //             else
    //             {
    //                 throw;
    //             }
    //         }
    //         return RedirectToAction("TableData");
    //     }
    //     return View(satellite);
    // }
    //
    // // Метод для удаления записи
    // public async Task<IActionResult> Delete(int? id)
    // {
    //     if (id == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     var satellite = await DataBaseContext.Satellites
    //         .FirstOrDefaultAsync(m => m.IdSatellite == id);
    //     if (satellite == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     return View(satellite);
    // }
    //
    // [HttpPost, ActionName("Delete")]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> DeleteConfirmed(int id)
    // {
    //     var satellite = await DataBaseContext.Satellites.FindAsync(id);
    //     DataBaseContext.Satellites.Remove(satellite);
    //     await DataBaseContext.SaveChangesAsync();
    //     return RedirectToAction("TableData");
    // }

    private bool SatelliteExists(int id)
    {
        return DataBaseContext.Satellites.Any(e => e.IdSatellite == id);
    }
}