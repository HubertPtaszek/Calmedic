using Calmedic.Application;
using Calmedic.Dictionaries;
using Calmedic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Calmedic.Controllers
{
    public class DashboardController : AppController
    {
        #region Dependencies

        private readonly IDashboardService _dashboardService;

        #endregion Dependencies

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public ActionResult Index()
        {
            if (UserHelper.UserHaveRole(HttpContext, AppRoleType.Administrator))
            {
                return RedirectToAction("AdminDashboard");
            }
            else if (UserHelper.UserHaveRole(HttpContext, AppRoleType.Clinic))
            {
                return RedirectToAction("ClinicDashboard");
            }
            else if (UserHelper.UserHaveRole(HttpContext, AppRoleType.Reception))
            {
                return RedirectToAction("ReceptionDashboard");
            }
            return RedirectToAction("DoctorDashboard");
        }

        [AppRoleAuthorization(AppRoleType.Administrator)]
        public ActionResult AdminDashboard()
        {
            return View("AdminDashboard");
        }

        [AppRoleAuthorization(AppRoleType.Clinic)]
        public ActionResult ClinicDashboard()
        {
            ClinicDashboardVM model = new ClinicDashboardVM();
            return View("ClinicDashboard", model);
        }

        [AppRoleAuthorization(AppRoleType.Reception)]
        public ActionResult ReceptionDashboard()
        {
            return View("ReceptionDashboard");
        }

        [AppRoleAuthorization(AppRoleType.Doctor)]
        public ActionResult DoctorDashboard()
        {
            return View("DoctorDashboard");
        }
    }
}