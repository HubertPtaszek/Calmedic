using Calmedic.Application;
using Calmedic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Calmedic.Areas.Evidence.Controllers
{
    [Area(AreaNames.Evidence_Area)]
    public class VisitController : AppController
    {
        public IActionResult Index()
        {
            return View(new VisitListVM());
        }
    }
}