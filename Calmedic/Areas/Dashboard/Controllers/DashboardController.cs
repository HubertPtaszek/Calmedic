using Calmedic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Calmedic.Areas.Dashboard.Controllers
{
    [Area(AreaNames.Dashboard_Area)]
    public class DashboardController : AppController
    {
        #region Dependencies

        #endregion


        public ActionResult Index()
        {
            return View();
        }
    }
}
