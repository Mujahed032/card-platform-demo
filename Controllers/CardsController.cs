using Microsoft.AspNetCore.Mvc;
using CardPlatformDemo.Models;
using CardPlatformDemo.Services;

namespace CardPlatformDemo.Controllers;

public class CardsController : Controller
{
    private readonly CardCatalog _catalog;

    public CardsController(CardCatalog catalog)
    {
        _catalog = catalog;
    }

    // Card detail + lifecycle walkthrough
    public IActionResult Details(string id)
    {
        var product = _catalog.GetProduct(id);
        if (product == null) return NotFound();

        ViewBag.Lifecycle = _catalog.GetLifecycle(id);
        return View(product);
    }
}
