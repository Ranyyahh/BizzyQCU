using Microsoft.AspNetCore.Mvc;

namespace IPT_Juvi.Web.Controllers;

public sealed class EnterprisesController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}

