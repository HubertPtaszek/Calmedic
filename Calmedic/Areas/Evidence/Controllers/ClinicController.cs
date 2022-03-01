using Calmedic.Application;
using Calmedic.Dictionaries;
using Calmedic.Utils;
using DevExtreme.AspNet.Mvc;
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
                return RedirectToAction("ClinicDetails");
            }
        }

        [HttpGet]
        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Administrator, AppRoleType.Doctor })]
        public ActionResult GetData(DataSourceLoadOptions loadOptions)
        {
            object data = _clinicService.GetClinics(loadOptions, HttpContext);
            return CustomJson(data);
        }

        [HttpGet]
        [AppRoleAuthorization(AppRoleType.Administrator)]
        public ActionResult GetClinicDocotrs(DataSourceLoadOptions loadOptions, int clinicId)
        {
            var data = _clinicService.GetClinicDocotrs(loadOptions, clinicId);
            return CustomJson(data);
        }

        [AppRoleAuthorization(AppRoleType.Administrator)]
        public ActionResult Add()
        {
            return View(new ClinicAddVM());
        }

        [HttpPost, ValidateAntiForgeryToken]
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

        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Administrator, AppRoleType.Doctor })]
        public ActionResult Details(int id)
        {
            ClinicDetailsVM model = _clinicService.GetClinicDetailsVM(id);
            return View(model);
        }

        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Clinic, AppRoleType.Reception })]
        public ActionResult ClinicDetails()
        {
            ClinicDetailsVM model = _clinicService.GetClinicDetailsVMForUser(HttpContext);
            return View("ClinicDetails", model);
        }

        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Administrator, AppRoleType.Clinic })]
        public ActionResult Edit(int id)
        {
            ClinicEditVM model = _clinicService.GetClinicEditVM(id);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AppRoleAuthorization(new AppRoleType[] { AppRoleType.Administrator, AppRoleType.Clinic })]
        public ActionResult Edit(ClinicEditVM model)
        {
            if (ModelState.IsValid)
            {
                string path = _webHostEnvironment.WebRootPath;
                //zapis logo + dodanie konta dla przychodnii
                int id = _clinicService.Edit(model);
                if (UserHelper.UserHaveRole(HttpContext, AppRoleType.Administrator) || UserHelper.UserHaveRole(HttpContext, AppRoleType.Doctor))
                {
                    return RedirectToAction("Details", new { id = model.Id });
                }
                else
                {
                    return RedirectToAction("ClinicDetails");
                }
            }
            return View(model);
        }

        [HttpDelete, ValidateAntiForgeryToken]
        [AppRoleAuthorization(AppRoleType.Administrator)]
        public ActionResult Delete(int id)
        {
            return CustomJson(true);
        }
    }
}