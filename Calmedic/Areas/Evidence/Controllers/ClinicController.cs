using Calmedic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Calmedic.Areas.Evidence.Controllers
{
    [Area(AreaNames.Evidence_Area)]
    public class ClinicController : AppController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}