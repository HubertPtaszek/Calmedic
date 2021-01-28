using Calmedic.Application;
using Calmedic.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calmedic.Areas.Presentation.Controllers
{
    [Area(AreaNames.Presentation_Area)]
    public class PlayerController : AppController
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(new PlayerVM());
        }
    }
}