using Microsoft.AspNetCore.Mvc;

namespace IPT_Juvi.Web.Controllers;

public sealed class ProductsController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}

