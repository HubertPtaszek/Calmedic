using Calmedic.Application;
using Calmedic.Dictionaries;
using Calmedic.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Calmedic.Areas.Evidence.Controllers
{
    [Area(AreaNames.Evidence_Area)]
    public class ClinicController : AppController
    {
        #region Dependencies

        private IWebHostEnvironment _webHostEnvironment;
        private readonly IClinicService _clinicService;

        #endregion Dependencies

        public ClinicController(IWebHostEnvironment webHostEnvironment, IClinicService clinicService)
        {
            _webHostEnvironment = webHostEnvironment;
            _clinicService = clinicService;
        }

        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Administrator, AppRoleType.Clinic, AppRoleType.Reception, AppRoleType.Doctor })]
        public IActionResult Index()
        {
            if (UserHelper.UserHaveRole(HttpContext, AppRoleType.Administrator) || UserHelper.UserHaveRole(HttpContext, AppRoleType.Doctor))
            {
                ClinicListVM model = _clinicService.GetClinicListVM();
                return View("Index", model);
            }
            else
            {
                ClinicDetailsVM model = _clinicService.GetClinicDetailsVMForUser(HttpContext);
                return View("Details", model);
            }
        }

        public ActionResult Add()
        {
            ClinicAddVM model = _clinicService.GetClinicAddVM();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppRoleAuthorization(AppRoleType.Administrator)]
        public ActionResult Add(ClinicAddVM model)
        {
            if (ModelState.IsValid)
            {
                string path = _webHostEnvironment.WebRootPath;
                //zapis logo + dodanie konta dla przychodnii
                int id = _clinicService.Add(model);
                return RedirectToAction("Details", new { id = id });
            }
            return View(model);
        }
    }
}