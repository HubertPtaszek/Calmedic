using Calmedic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Calmedic.Areas.DisplaySequence.Controllers
{
    [Area(AreaNames.DisplaySequence_Area)]
    public class DisplaySequenceController : AppController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}