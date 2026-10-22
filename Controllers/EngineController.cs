using Microsoft.AspNetCore.Mvc;
using CardPlatformDemo.Models;
using CardPlatformDemo.Services;

namespace CardPlatformDemo.Controllers;

public class EngineController : Controller
{
    private readonly CardCatalog _catalog;

    public EngineController(CardCatalog catalog)
    {
        _catalog = catalog;
    }

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.RuleTable = _catalog.GetRuleTable();
        ViewBag.Products = _catalog.Products;
        ViewBag.Decision = null;
        return View(new TransactionInput());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(TransactionInput input)
    {
        ViewBag.RuleTable = _catalog.GetRuleTable();
        ViewBag.Products = _catalog.Products;
        ViewBag.Decision = _catalog.Evaluate(input);
        return View(input);
    }
}
