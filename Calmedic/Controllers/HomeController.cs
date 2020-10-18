using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Calmedic.Models;
using Calmedic.Utils;
using Microsoft.AspNetCore.Authorization;

namespace Calmedic.Controllers
{
    public class HomeController : AppController
    {
        public HomeController()
        { }

        [AllowAnonymous]
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = AreaNames.Membership_Area });
            }
            return RedirectToAction("Index", "Dashboard", new { area = AreaNames.Dashboard_Area });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
