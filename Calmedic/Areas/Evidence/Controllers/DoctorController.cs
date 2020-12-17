using Calmedic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Calmedic.Areas.Evidence.Controllers
{
    [Area(AreaNames.Evidence_Area)]
    public class DoctorController : AppController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}