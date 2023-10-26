using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Abstraction;
using Web.Models;

namespace Web.Controllers;

public class HomeController : AbstractController<HomeController>
{
    public HomeController(ILogger<HomeController> logger) : base(logger)
    {
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("ErrorPage/Error",
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult ErrorCodePage(string errorCode)
    {
        return int.TryParse(errorCode, out var code)
            ? View("ErrorPage/ErrorCodePage", code is >= 100 and <= 510 ? errorCode : "404")
            : View("ErrorPage/ErrorCodePage", "404");
    }
}