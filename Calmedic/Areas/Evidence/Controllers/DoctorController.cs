using Calmedic.Dictionaries;
using Calmedic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Calmedic.Areas.Evidence.Controllers
{
    [Area(AreaNames.Evidence_Area)]
    public class DoctorController : AppController
    {
        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Administrator, AppRoleType.Clinic, AppRoleType.Reception })]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Administrator, AppRoleType.Clinic, AppRoleType.Reception })]
        public IActionResult GetDate()
        {
            return CustomJson(new object());
        }
    }
}