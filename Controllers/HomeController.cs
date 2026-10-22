using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CardPlatformDemo.Models;
using CardPlatformDemo.Services;

namespace CardPlatformDemo.Controllers;

public class HomeController : Controller
{
    private readonly CardCatalog _catalog;

    public HomeController(CardCatalog catalog)
    {
        _catalog = catalog;
    }

    public IActionResult Index()
    {
        return View(_catalog);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
