using Calmedic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Calmedic.Areas.Membership.Controllers
{
    [Area(AreaNames.Membership_Area)]
    public class AppUserController : AppController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}