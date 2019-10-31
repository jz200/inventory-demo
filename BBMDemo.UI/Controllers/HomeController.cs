using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BBMDemo.UI.Models;
using Microsoft.AspNetCore.Authorization;

namespace BBMDemo.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public HomeController()
        { }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "ProductInventories");
        }

        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
