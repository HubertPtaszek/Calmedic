using Calmedic.Application;
using Calmedic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Calmedic.Areas.Dashboard.Controllers
{
    [Area(AreaNames.Dashboard_Area)]
    public class DashboardController : AppController
    {
        #region Dependencies

        private readonly IPatientService _patientService;

        #endregion Dependencies

        public DashboardController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Index");
        }
    }
}
